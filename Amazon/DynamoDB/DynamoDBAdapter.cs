using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Internal;
using Amazon.Runtime;
using Amazon.S3;

namespace Lab03.Amazon.DynamoDB
{
    //todo add Identity
    public class DynamoDBAdapter
    {
        private readonly string accessId = ConfigurationManager.AppSettings["accessId"];
        private readonly string secretKey = ConfigurationManager.AppSettings["secretKey"];
        private readonly string endpoint = ConfigurationManager.AppSettings["endpoint"];
        private BasicAWSCredentials _credentials;
        private AmazonDynamoDBClient _client;

        public DynamoDBAdapter()
        {
            _credentials = new BasicAWSCredentials(accessId, secretKey);
            AWSConfigs.EndpointDefinition = endpoint;
            _client = new AmazonDynamoDBClient(_credentials,RegionEndpoint.USEast1);
            // createTable("FileCommenter");
        }

        private async Task createTable(String name)
        {
            CreateTableRequest request = new CreateTableRequest
            {
                TableName = name,
                AttributeDefinitions = new List<AttributeDefinition>()
                {
                    new AttributeDefinition()
                    {
                        AttributeName = "filename",
                        AttributeType = "S",
                    },
                    new AttributeDefinition()
                    {
                        AttributeName = "comment",
                        AttributeType = "S",
                    },
                },
                KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement
                    {
                        AttributeName = "filename",
                        KeyType = "HASH"
                    },
                    new KeySchemaElement
                    {
                        AttributeName = "comment",
                        KeyType = "RANGE"
                    }
                },
                BillingMode = BillingMode.PROVISIONED,
                ProvisionedThroughput = new ProvisionedThroughput(1, 1)
            };

            try
            {
                await _client.CreateTableAsync(request);
                Console.WriteLine("table created successfully");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }

        public async Task insertItems(string filename, string comment)
        {
            PutItemRequest request = new PutItemRequest
            {
                TableName = "FileCommenter",
                Item = new Dictionary<string, AttributeValue>
                {
                    {"filename", new AttributeValue {S = filename}},
                    {"comment", new AttributeValue {S = comment}},
                }
            };

            await _client.PutItemAsync(request);
        }
    }
}