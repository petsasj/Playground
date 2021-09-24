using System;
using NBomber.Contracts;
using NBomber.CSharp;
using NBomber.Plugins.Http.CSharp;

namespace Playground.LoadTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var httpFactory = HttpClientFactory.Create();

            var step = Step.Create("step1", httpFactory, async context =>
            {
                var response =
                    await context.Client.GetAsync("https://localhost:5001/pdf/getpdf",
                                                  context.CancellationToken);
                
                
                return response.IsSuccessStatusCode
                    ? Response.Ok(statusCode: (int)response.StatusCode)
                    : Response.Fail(statusCode: (int)response.StatusCode);
            },timeout: TimeSpan.FromSeconds(2));
            
            var scenario = ScenarioBuilder
                           .CreateScenario("simple_http", step)
                           .WithWarmUpDuration(TimeSpan.FromSeconds(20))
                           .WithLoadSimulations(
                               Simulation.KeepConstant(1, TimeSpan.FromSeconds(5)),
                               Simulation.InjectPerSec(rate: 1, during: TimeSpan.FromSeconds(30))
                           );

            NBomberRunner
                .RegisterScenarios(scenario)
                .Run();
        }
    }
}