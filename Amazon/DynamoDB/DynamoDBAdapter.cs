using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Amazon.Internal;
using Amazon.Runtime;
using Amazon.S3;

namespace Lab03.Amazon.DynamoDB
{
    public class DynamoDBAdapter
    {
        private readonly string accessId = ConfigurationManager.AppSettings["accessId"];
        private readonly string secretKey = ConfigurationManager.AppSettings["secretKey"];
        private readonly string endpoint = ConfigurationManager.AppSettings["endpoint"];
        private BasicAWSCredentials _credentials;
        private AmazonDynamoDBClient _client;
        private bool isAuthenticUser = false;

        public DynamoDBAdapter()
        {
            _credentials = new BasicAWSCredentials(accessId, secretKey);
            AWSConfigs.EndpointDefinition = endpoint;
            _client = new AmazonDynamoDBClient(_credentials, RegionEndpoint.USEast2);
            // createTable("FileCommenter");
            // could not implement the User Login and sign up 
            // createTable("UsersTable", "username", "password");
            // insertUser( "abhishek", "123");
        }

        private async Task createTable(String name, string col1, string col2)
        {
            CreateTableRequest request = new CreateTableRequest
            {
                TableName = name,
                AttributeDefinitions = new List<AttributeDefinition>()
                {
                    new AttributeDefinition()
                    {
                        AttributeName = col1,
                        AttributeType = "S",
                    },
                    new AttributeDefinition()
                    {
                        AttributeName = col2,
                        AttributeType = "S",
                    },
                },
                KeySchema = new List<KeySchemaElement>
                {
                    new KeySchemaElement
                    {
                        AttributeName = col1,
                        KeyType = "HASH"
                    },
                    new KeySchemaElement
                    {
                        AttributeName = col2,
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

        public bool isAuthenticated(string username ,string password)
        {
            isAuthenticated(username, password);
            return isAuthenticUser;
        }
        public async Task authenticateUser(string username, string password)
        {
            GetItemRequest request = new GetItemRequest
            {
                TableName = "UsersTable",
                Key = new Dictionary<string, AttributeValue>()
                {
                    {"username", new AttributeValue(username)},
                    {"password", new AttributeValue(password)},
                }
            };

            GetItemResponse response = await _client.GetItemAsync(request);

            if (response.HttpStatusCode == HttpStatusCode.OK)
            {
                if (response.Item.Count > 0)
                {
                    isAuthenticUser = true;
                }
            }

            isAuthenticUser =false;
        }
        
        public async Task insertUser ( string val1, string val2 )
        {
            PutItemRequest request = new PutItemRequest
            {
                TableName = "UsersTable",
                Item = new Dictionary<string, AttributeValue>
                {
                    {"username", new AttributeValue {S = val1}},
                    {"password", new AttributeValue {S = val2}},
                }
            };

            await _client.PutItemAsync(request);
        }

        public async Task insertFile(string val1, string val2)
        {
            PutItemRequest request = new PutItemRequest
            {
                TableName = "FileCommenter",
                Item = new Dictionary<string, AttributeValue>
                {
                    {"filename", new AttributeValue {S = val1}},
                    {"comment", new AttributeValue {S = val2}},
                }
            };

            await _client.PutItemAsync(request);
        }
    }
}