
using PaymentProcessorApi.Services;

namespace PaymentProcessorApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddAuthorization();
            builder.Services.AddOpenApi();
            builder.Services.AddHttpClient<IPaymentGatewayService, PaymentGatewayService>(client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["PaymentGateway:BaseUrl"]!);
            });
            builder.Services.AddSingleton<IIdempotencyService, InMemoryIdempotencyService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
