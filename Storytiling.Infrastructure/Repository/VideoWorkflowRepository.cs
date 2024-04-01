using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Microsoft.Extensions.Logging;
using Storytiling.ApplicationCore.DTOs;
using Storytiling.ApplicationCore.Repository;
using Storytiling.Infrastructure.Common;

namespace Storytiling.Infrastructure.Repository
{
    internal class VideoWorkflowRepository : IVideoWorkflowRepository
    {
        private readonly ILogger<VideoWorkflowRepository> _logger;
        private readonly IAmazonDynamoDB _client;
        private const string TableName = "VideoWorkflow";
        public VideoWorkflowRepository(ILogger<VideoWorkflowRepository> logger, IAmazonDynamoDB amazonDynamoDB)
        {
            this._logger = logger;
            this._client = amazonDynamoDB;
        }
        /// <summary>
        /// This method retrive the items from VideoWorkflow table, filtering by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<VideoWorkflowDTO?> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var request = new QueryRequest
                {
                    TableName = TableName,
                    KeyConditionExpression = $"VideoWorkflowId = :videoWorkflowId",
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                    {
                        {":videoWorkflowId",new AttributeValue{ S = id.ToString() }},
                    }
                };
                var response = await _client.QueryAsync(request, cancellationToken);
                var videoWorkflowDto = response.Items.Select(e => e.ToVideoWorkflowDTO());
                return videoWorkflowDto.FirstOrDefault()!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Class:{nameof(VideoWorkflowRepository)}, Method:{GetAsync}");
                return null;
            }
        }
        /// <summary>
        /// This method retives all video workflow items from table
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IEnumerable<VideoWorkflowDTO>> GetAllAsync(CancellationToken cancellationToken)
        {
            try
            {
                var request = new ScanRequest
                {
                    TableName = TableName,
                };
                var response = await _client.ScanAsync(request,cancellationToken);
                var videoWorkflowDto = response.Items.Select(e => e.ToVideoWorkflowDTO());
                return videoWorkflowDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Class:{nameof(VideoWorkflowRepository)}, Method:{GetAllAsync}");
                return Enumerable.Empty<VideoWorkflowDTO>();
            }
        }
        /// <summary>
        /// This method retrive the items from VideoWorkflow table, filtering by employeeId
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public async Task<VideoWorkflowDTO?> GetByEmployeeIdAsync(int employeeId, CancellationToken cancellationToken)
        {
            try
            {
                var request = new ScanRequest
                {
                    TableName = TableName,
                    FilterExpression = $"EmployeeId = :employeeId",
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                    {
                        {":employeeId",new AttributeValue{ N = employeeId.ToString() }},
                    }
                };
                var response = await _client.ScanAsync(request, cancellationToken);
                var videoWorkflowDto = response.Items.Select(e => e.ToVideoWorkflowDTO());
                return videoWorkflowDto.FirstOrDefault()!;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Class:{nameof(VideoWorkflowRepository)}, Method:{GetByEmployeeIdAsync}");
                return null;
            }
        }
        /// <summary>
        /// This method creates a new item inside VideoWorkflow table
        /// </summary>
        /// <param name="videoWorkflowDTO"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<VideoWorkflowDTO> CreateAsync(VideoWorkflowDTO videoWorkflowDTO, CancellationToken cancellationToken)
        {
            try
            {
                var request = new PutItemRequest
                {
                    TableName = TableName,
                    Item = videoWorkflowDTO.ToDynamoDBItems()
                };
                var response = await _client.PutItemAsync(request, cancellationToken);
                return videoWorkflowDTO;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Class:{nameof(VideoWorkflowRepository)}, Method:{CreateAsync}");
                throw;
            }
        }
        /// <summary>
        /// This method updates the VideoWorkflow item
        /// </summary>
        /// <param name="videoWorkflowDTO"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<VideoWorkflowDTO?> UpdateAsync(VideoWorkflowDTO videoWorkflowDTO, CancellationToken cancellationToken)
        {
            try
            {
                var request = new UpdateItemRequest
                {
                    TableName = TableName,
                    Key = new Dictionary<string, AttributeValue>
                    {
                        {"VideoWorkflowId",new AttributeValue{S=videoWorkflowDTO.VideoWorkflowId.ToString() }}
                    },
                    UpdateExpression = "SET ContributionPending = :ContributionPending, ContributionMade = :ContributionMade, VideoStatus = :VideoStatus, Title = :Title, EmployeeId = :EmployeeId, SubmissionDate = :SubmissionDate,Contributions = :Contributions",
                    ExpressionAttributeValues = new Dictionary<string, AttributeValue>
                    {
                        {":EmployeeId",new AttributeValue{N=videoWorkflowDTO.EmployeeId.ToString() }},
                        {":SubmissionDate",new AttributeValue{S=videoWorkflowDTO.SubmissionDate.ToString() }},
                        {":ContributionMade",new AttributeValue{N=videoWorkflowDTO.ContributionsMade.ToString() }},
                        {":ContributionPending",new AttributeValue{N=videoWorkflowDTO.ContributionsPending.ToString() }},
                        {":VideoStatus",new AttributeValue{N=((int)videoWorkflowDTO.Status).ToString() }},
                        {":Title",new AttributeValue{S=videoWorkflowDTO.Title }},
                        {":Contributions",new AttributeValue{L=videoWorkflowDTO.Contributions.Select(x=> new AttributeValue{M=x.ToDynamoDBItems()}).ToList() }},
                    },
                };
                var response = await _client.UpdateItemAsync(request, cancellationToken);

                return await GetByEmployeeIdAsync(videoWorkflowDTO.EmployeeId, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Class:{nameof(VideoWorkflowRepository)}, Method:{UpdateAsync}");
                throw;
            }
        }
    }
}
