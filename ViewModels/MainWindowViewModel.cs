using AppFox.Models;
using AppFox.Views;
using Avalonia;
using Avalonia.Controls;
using Newtonsoft.Json;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace AppFox.ViewModels
{

    public class MainWindowViewModel : ViewModelBase
    {
       public MainWindowViewModel()
        {
            StartDate = "2022.09.05";
            EndDate = "2022.09.06";
            ErrorText = "";
        }

        /// <summary>
        /// Делаем скриншот и отправляем на сервер
        /// </summary>
        public void MakeScreenshot()
        {
            Rectangle rect = new Rectangle(0, 0, Width, Height);
            Bitmap bmp = new Bitmap(rect.Width, rect.Height, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bmp);
            g.CopyFromScreen(rect.Left, rect.Top, 0, 0, bmp.Size, CopyPixelOperation.SourceCopy);

            var byteImage = BitToByte(bmp);

            Task task = UploadImage(byteImage);
        }
        /// <summary>
        /// Конвентируем Bitmap в Byte[]
        /// </summary>
        private static Byte[] BitToByte(Bitmap bitmap)
        {
            if (bitmap!=null)
            {
                using(MemoryStream ms = new MemoryStream())
                {
                    bitmap.Save(ms, ImageFormat.Jpeg);
                    return ms.ToArray();
                }
            }
            return null;
        }

        /// <summary>
        /// Загрузка скриншота
        /// </summary>
        /// <param name="file">Скриншот для загрузки</param>
        /// <returns></returns>
        private async Task UploadImage(Byte[] file)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                MultipartFormDataContent form = new MultipartFormDataContent();

                var fileName = $"file_{DateTime.Now.ToShortDateString()}_{DateTime.Now.ToShortTimeString()}";
                form.Add(new ByteArrayContent(file), "file", fileName);
                HttpResponseMessage response = await httpClient.PostAsync("http://45.84.226.180/UploadScreenshot", form);

                response.EnsureSuccessStatusCode();
                httpClient.Dispose();              
            }
            catch
            {
            }         
        }

        /// <summary>
        /// Позволяет получить все скриншоты за период
        /// </summary>
        /// <param name="startDate">Дата начала периода</param>
        /// <param name="endDate">Дата окончания периода</param>
        /// <returns></returns>
        private async Task GetScreenshots(string startDate, string endDate)
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                

                var requestScreenshot = new RequestScreenshot
                {
                    startDate = startDate,
                    endDate = endDate
                };

                var json = JsonConvert.SerializeObject(requestScreenshot);

                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("http://45.84.226.180/GetScreenshots"),
                    Content = new StringContent(json, Encoding.UTF8, "application/json"),
                };

                var response = await client.SendAsync(request).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();

                httpClient.Dispose();
            }
            catch
            {
            }
        }

        public void OnFirstCommand()
        {
            MakeScreenshot();
        }

        public void OnSecondCommand()
        {

            Task task = GetScreenshots(StartDate, EndDate);
        }
        
        public int Height { get; set; }
        public int Width { get; set; }

        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string ErrorText { get; set; }

        static readonly HttpClient client = new HttpClient();
    }

}
