using System.IO;
using System.Threading.Tasks;

namespace LeadPro.Model
{
    public interface IImageCaptureService
    {
        Task<Stream> CaptureAsync();
    }
}