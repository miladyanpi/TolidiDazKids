namespace Api.Services.HangfireServices
{
    public class ApplicationPreload : Microsoft.AspNetCore.Hosting.IApplicationLifetime
    {
        public CancellationToken ApplicationStarted => throw new NotImplementedException();

        public CancellationToken ApplicationStopping => throw new NotImplementedException();

        public CancellationToken ApplicationStopped => throw new NotImplementedException();

        public void StopApplication()
        {
            throw new NotImplementedException();
        }
    }
}
