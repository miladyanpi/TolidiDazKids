namespace Dto.Models.ResponseApi
{
    public class ResponseApiEntities<T> where T : class
    {
      
        public string? Status { get; set; }
        public int? StatusCode { get; set; }

        public string? Message { get; set; }
        public IEnumerable<T>? Entities { get; set; }
        public int CountAllRecordTable { get; set; }= 0;
        public ResponseApiEntities(IEnumerable<T>? entities, string? status, int statusCode, string? message, int countAllRecordTable = 0)
        {
            Status = status;
            StatusCode = statusCode;
            Entities = entities;
            Message = message;
            CountAllRecordTable = countAllRecordTable;
        }
    }
}
