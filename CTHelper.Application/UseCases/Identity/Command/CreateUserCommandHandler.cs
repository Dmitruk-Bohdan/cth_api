using CTHelper.Application.ServiceInterfaces;
using CTHelper.Domain.Abstractions;
using CTHelper.Domain.Entities;
using MapsterMapper;
using MediatR;

namespace Library.Application.AuthorUseCases.Commands;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, long>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IMapper _mapper;

    public CreateUserCommandHandler(
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _mapper = mapper;
    }

    public async Task<long> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var newUser = _mapper.Map<User>(request);
        newUser.PasswordHash = _passwordHasher.Hash(request.Password);
        
        await _unitOfWork.Users.AddAsync(newUser, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return newUser.Id;
    }
}
