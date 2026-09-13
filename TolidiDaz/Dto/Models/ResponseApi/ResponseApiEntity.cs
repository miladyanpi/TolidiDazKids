namespace Dto.Models.ResponseApi
{
   
    public class ResponseApiEntity<T> where T : class
    {
        public int ID { get; set; }
        public string? Status { get; set; }
        public int? StatusCode { get; set; }
        public T? Entity { get; set; }
        public string? Message { get; set; }
        public int? Count { get; set; }
        public ResponseApiEntity(T? entity, string? status, string? message,int statusCode,int id=0,int? count=0)
        {
            Status=status;
            StatusCode= statusCode;
            Entity=entity;
            Message=message;
            ID=id;
            Count = count;
        }
    }
   

}
