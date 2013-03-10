using System.IO;
using System.Threading.Tasks;
using LeadPro.Model;
using Microsoft.Phone.Tasks;

namespace LeadPro.Phone8.Services
{
    public class PhoneImageCaptureService : IImageCaptureService
    {
        public Task<Stream> CaptureAsync()
        {
            var cameraCompletionTask = new TaskCompletionSource<Stream>();
            var cameraCaptureTask = new CameraCaptureTask();
            cameraCaptureTask.Completed += (s, e) =>
                {
                    if (e.Error != null)
                    {
                        cameraCompletionTask.SetException(e.Error);
                    }

                    cameraCompletionTask.SetResult(e.ChosenPhoto);
                };
            
            Task.Run(() => cameraCaptureTask.Show());
            return cameraCompletionTask.Task;
        }
    }
}