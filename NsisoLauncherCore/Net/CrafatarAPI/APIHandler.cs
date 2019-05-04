using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace NsisoLauncherCore.Net.CrafatarAPI
{
    public class APIHandler
    {
        const string APIUrl = "https://crafatar.com/avatars/";
        public async Task<ImageSource> GetHeadSculSource(string uuid)
        {
            string url = APIUrl + uuid + "?size=60";
            var res = await APIRequester.HttpGetAsync(url);
            //var stream = await res.Content.ReadAsStreamAsync();
            if (res.IsSuccessStatusCode)
            {
                using (Stream stream = await res.Content.ReadAsStreamAsync())
                {
                    var bImage = new BitmapImage();
                    bImage.BeginInit();
                    bImage.StreamSource = stream;
                    bImage.EndInit();
                    return bImage;
                }
            }
            else
            {
                return new BitmapImage(new Uri("/NsisoLauncher;component/Resource/Steve.jpg"));
            }
        }
    }
}
