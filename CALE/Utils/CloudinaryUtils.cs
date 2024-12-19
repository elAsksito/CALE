using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using dotenv.net;
using System;

namespace CALE.Utils
{
    public static class CloudinaryUtils
    {
        private static Cloudinary cloudinary;

        static CloudinaryUtils()
        {
            DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));
            cloudinary = new Cloudinary(Environment.GetEnvironmentVariable("CLOUDINARY_URL"));
            cloudinary.Api.Secure = true;
        }

        public static void UploadImage(string imageUrl)
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(imageUrl),
                UseFilename = true,
                UniqueFilename = false,
                Overwrite = true
            };

            var uploadResult = cloudinary.Upload(uploadParams);
        }

        public static ImageUploadResult UploadImageFromFile(IFormFile file)
        {
            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                stream.Position = 0;

                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, stream),
                    UseFilename = true,
                    UniqueFilename = true,
                    Overwrite = true,
                    Folder = "CALE"
                };

                var uploadResult = cloudinary.Upload(uploadParams);
                return uploadResult;
            }
        }

        public static void GetImageDetails(string publicId)
        {
            var getResourceParams = new GetResourceParams(publicId)
            {
                QualityAnalysis = true
            };

            var getResourceResult = cloudinary.GetResource(getResourceParams);
            var resultJson = getResourceResult.JsonObj;
        }

        public static void TransformImage(string publicId)
        {
            var myTransformation = cloudinary.Api.UrlImgUp.Transform(new Transformation()
                .Width(300).Crop("scale").Chain()
                .Effect("cartoonify"));

            var myUrl = myTransformation.BuildUrl(publicId);
            var myImageTag = myTransformation.BuildImageTag(publicId);
        }
    }
}
