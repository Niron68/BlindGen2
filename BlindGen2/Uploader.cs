using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BlindGen2
{
    public class Uploader
    {
        public async Task Upload(VideoInfo info)
        {
            UserCredential credential;
            using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new[] { YouTubeService.Scope.YoutubeUpload },
                    "user",
                    CancellationToken.None,
                    new FileDataStore(this.GetType().ToString())
                );
            }

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "BlindGen"
            });

            var video = new Video();
            video.Snippet = new VideoSnippet();
            video.Snippet.Title = info.Title;
            video.Snippet.Description = info.Description;
            video.Snippet.Tags = info.Tags;
            video.Snippet.CategoryId = "22";
            video.Status = new VideoStatus();
            video.Status.PrivacyStatus = info.Privacy;
            var filePath = info.Path;

            using(var fileStream = new FileStream(filePath, FileMode.Open))
            {
                var videoInsertRequest = youtubeService.Videos.Insert(video, "snippet,status", fileStream, "video/*");
                videoInsertRequest.ProgressChanged += videoInsertRequest_ProgressChanged;
                videoInsertRequest.ResponseReceived += VideoInsertRequest_ResponseReceived;

                await videoInsertRequest.UploadAsync();
            }
        }

        private void videoInsertRequest_ProgressChanged(IUploadProgress progress)
        {
            switch (progress.Status)
            {
                case UploadStatus.Uploading:
                    Console.WriteLine("{0} bytes sent.", progress.BytesSent);
                    break;

                case UploadStatus.Failed:
                    Console.WriteLine("An error prevented the upload from completing.\n{0}", progress.Exception);
                    break;
            }
        }

        private void VideoInsertRequest_ResponseReceived(Video video)
        {
            Console.WriteLine("Video id '{0}' was succesfully uploaded.", video.Id);
        }
    }
}
