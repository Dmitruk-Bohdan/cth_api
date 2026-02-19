namespace CTHelper.Presentation.Extensions
{
    public static class WebApplicationExtension
    {
        public static void ConfigureMiddleware(this WebApplication app)
        {
            app.UseGlobalExceptionHandling();

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseCors("FrontendPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CTHelper API V1");
                    c.RoutePrefix = "swagger";
                });
            }
        }
    }
}
