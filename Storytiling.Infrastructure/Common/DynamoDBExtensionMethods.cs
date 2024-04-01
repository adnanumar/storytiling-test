using Amazon.DynamoDBv2.Model;
using Storytiling.ApplicationCore.DTOs;

namespace Storytiling.Infrastructure.Common
{
    public static class DynamoDBExtensionMethods
    {
        /// <summary>
        /// This extension method mapps VideoWorkflowDTO to DynamoDBItems
        /// </summary>
        /// <param name="videoWorkflowDTO"></param>
        /// <returns></returns>
        public static Dictionary<string, AttributeValue> ToDynamoDBItems(this VideoWorkflowDTO videoWorkflowDTO)
        {
            var items = new Dictionary<string, AttributeValue>
            {
                {"VideoWorkflowId",new AttributeValue{S=videoWorkflowDTO.VideoWorkflowId.ToString() }},
                {"EmployeeId",new AttributeValue{N=videoWorkflowDTO.EmployeeId.ToString() }},
                {"SubmissionDate",new AttributeValue{S=videoWorkflowDTO.SubmissionDate.ToString() }},
                {"ContributionMade",new AttributeValue{N=videoWorkflowDTO.ContributionsMade.ToString() }},
                {"ContributionPending",new AttributeValue{N=videoWorkflowDTO.ContributionsPending.ToString() }},
                {"VideoStatus",new AttributeValue{N=((int)videoWorkflowDTO.Status).ToString() }},
                {"Title",new AttributeValue{S=videoWorkflowDTO.Title }}
            };
            if (videoWorkflowDTO.Contributions.Any())
            {
                items.Add("Contributions", new AttributeValue { L = videoWorkflowDTO.Contributions.Select(x => new AttributeValue { M = x.ToDynamoDBItems() }).ToList() });
            }
            return items;
        }
        /// <summary>
        /// This extension method mapps VideoWorkflowContributionDTO to DynamoDBItems
        /// </summary>
        /// <param name="videoWorkflowContributionDTO"></param>
        /// <returns></returns>
        public static Dictionary<string, AttributeValue> ToDynamoDBItems(this VideoWorkflowContributionDTO videoWorkflowContributionDTO)
        {
            return new Dictionary<string, AttributeValue>
            {
                {"ContributerId",new AttributeValue{N=videoWorkflowContributionDTO.ContributerId.ToString() }},
                {"VideoId",new AttributeValue{N=videoWorkflowContributionDTO.VideoId.ToString() }},
                {"VideoUrl",new AttributeValue{S=videoWorkflowContributionDTO.VideoUrl.ToString() }},
                {"ContributionStatus",new AttributeValue{N=((int)videoWorkflowContributionDTO.Status).ToString() }},
                {"Timestamp",new AttributeValue{S=videoWorkflowContributionDTO.Timestamp.ToString() }},
            };
        }
        /// <summary>
        /// This extension methods mapps DynamoDBItems to VideoWorkflowDTO
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static VideoWorkflowDTO ToVideoWorkflowDTO(this Dictionary<string, AttributeValue> items)
        {
            return new VideoWorkflowDTO
            {
                VideoWorkflowId = Guid.Parse(items["VideoWorkflowId"].S),
                EmployeeId = int.Parse(items["EmployeeId"].N),
                SubmissionDate = DateTime.Parse(items["SubmissionDate"].S),
                ContributionsMade = int.Parse(items["ContributionMade"].N),
                ContributionsPending = int.Parse(items["ContributionPending"].N),
                Status = (VideoWorkflowStatus)Enum.Parse(typeof(VideoWorkflowStatus), items["VideoStatus"].N),
                Title = items["Title"].S,
                Contributions = items.TryGetValue("Contributions",out var _contributions)?_contributions.L.Select(x => x.M.ToVideoWorkflowContributionDTO()).ToList():new List<VideoWorkflowContributionDTO>()
            };
        }
        /// <summary>
        /// This extension methods mapps DynamoDBItems to VideoWorkflowContributionDTO
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public static VideoWorkflowContributionDTO ToVideoWorkflowContributionDTO(this Dictionary<string, AttributeValue> items)
        {
            return new VideoWorkflowContributionDTO
            {
                ContributerId = int.Parse(items["ContributerId"].N),
                Timestamp = DateTime.Parse(items["Timestamp"].S),
                VideoId = int.Parse(items["VideoId"].N),
                VideoUrl = items["VideoUrl"].S,
                Status = (VideoWorkflowContributionStatus)Enum.Parse(typeof(VideoWorkflowContributionStatus), items["ContributionStatus"].N),
            };
        }
    }
}
