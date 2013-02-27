using System;
using System.IO;
using System.Threading.Tasks;
using LeadPro.Model;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;

namespace LeadPro.Win8.Services
{
    class WinRtImageCaptureService : IImageCaptureService
    {
        public async Task<Stream> CaptureAsync()
        {
            var dialog = new MessageDialog("Would you like to use your camera or select a picture from your library?");
            dialog.Commands.Add(new UICommand("I'd like to use my camera", null, "camera"));
            dialog.Commands.Add(new UICommand("I already have the picture", null, "picker"));

            IStorageFile photoFile;
            var command = await dialog.ShowAsync();
            if ((string)command.Id == "camera")
            {
                var cameraCapture = new CameraCaptureUI();
                photoFile = await cameraCapture.CaptureFileAsync(CameraCaptureUIMode.Photo);
            }
            else
            {
                var photoPicker = new FileOpenPicker();
                photoPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                photoPicker.FileTypeFilter.Add(".png");
                photoPicker.FileTypeFilter.Add(".jpg");
                photoPicker.FileTypeFilter.Add(".jpeg");

                photoFile = await photoPicker.PickSingleFileAsync();
            }

            if (photoFile == null)
                return null;

            var raStream = await photoFile.OpenAsync(FileAccessMode.Read);
            return raStream.AsStream();
        }
    }
}
