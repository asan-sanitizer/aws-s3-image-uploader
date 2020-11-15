using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;

namespace Lab03.Amazon.S3
{
    public class S3Adapter
    {
        private const string bucketName = "dropbox-lab03";
        private const string keyName = "comp306";
        private List<String> bucketsList;
        
        private readonly RegionEndpoint bucketRegion = RegionEndpoint.USEast1;
        private AmazonS3Client client;

        //need to update this 
        private readonly string accessId = ConfigurationManager.AppSettings["accessId"];
        private readonly string secretKey = ConfigurationManager.AppSettings["secretKey"];
        private BasicAWSCredentials _credentials;

        public S3Adapter()
        {
            _credentials = new BasicAWSCredentials(accessId, secretKey);
            client = new AmazonS3Client(_credentials, bucketRegion);
            bucketsList = new List<string>();
        }

        public async Task uploadFile(string filePath)
        {
            try
            {
                PutObjectRequest request = new PutObjectRequest
                {
                    BucketName = bucketName,
                    FilePath = filePath
                };

                PutObjectResponse response = await client.PutObjectAsync(request);
                
                Debug.WriteLine("File uploaded at" , generateDocumentUrl(1));

            }
            catch (AmazonS3Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            
        }

        public string generateDocumentUrl(double duration)
        {
            GetPreSignedUrlRequest request = new GetPreSignedUrlRequest()
            {
                BucketName = bucketName,
                Key = keyName,
                Verb = HttpVerb.PUT,
                Expires = DateTime.Now.AddHours(duration)
            };

            string url = client.GetPreSignedURL(request);
            
            return url;
        }

        public async Task ListBuckets()
        {
            try
            {
                ListBucketsResponse response = await client.ListBucketsAsync();

                foreach (var bucket in response.Buckets) bucketsList.Add(bucket.BucketName);
            }
            catch (AmazonS3Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        

        public List<string> getBuckets()
        {
            ListBuckets();
            return bucketsList;
        }
        

    }
}