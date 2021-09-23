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
                    await context.Client.GetAsync("http://20.101.217.162/weatherforecast/getpdf",
                                                  context.CancellationToken);
                
                
                return response.IsSuccessStatusCode
                    ? Response.Ok(statusCode: (int)response.StatusCode)
                    : Response.Fail(statusCode: (int)response.StatusCode);
            });
            
            var scenario = ScenarioBuilder
                           .CreateScenario("simple_http", step)
                           .WithWarmUpDuration(TimeSpan.FromSeconds(20))
                           .WithLoadSimulations(
                               Simulation.KeepConstant(1, TimeSpan.FromSeconds(5)),
                               Simulation.InjectPerSec(rate: 1, during: TimeSpan.FromSeconds(10))
                           );

            NBomberRunner
                .RegisterScenarios(scenario)
                .Run();
        }
    }
}