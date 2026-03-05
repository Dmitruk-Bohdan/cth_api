using CTHelper.Application.Services.Interfaces;
using CTHelper.Application.UseCases.Identity.Command;
using CTHelper.Domain.Abstractions;
using CTHelper.Domain.Entities;
using MapsterMapper;
using MediatR;

namespace Library.Application.AuthorUseCases.Commands;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, User>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHashService _passwordHasher;
    private readonly IMapper _mapper;

    public RegisterUserCommandHandler(
        IUnitOfWork unitOfWork,
        IHashService passwordHasher,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _mapper = mapper;
    }

    public async Task<User> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var newUser = _mapper.Map<User>(request);
        newUser.PasswordHash = _passwordHasher.Get128Hash(request.Password);
        
        await _unitOfWork.Users.AddAsync(newUser, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return newUser;
    }
}
