namespace CTHelper.Application.UseCases.TeacherStudentRelationship.Command;

public record RequestBindingWithTeacherByCodeCommand(long StudentId,
                                                     string Code);
