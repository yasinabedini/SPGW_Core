namespace SPGW.Domain.Common
{
    public class ServiceResult<T>
    {
        public bool Succeed { get; set; }

        public long ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public T Result { get; set; }


        public ServiceResult()
        {
            Succeed = true;
            ErrorCode = 0;
            ErrorMessage = "";
        }
    }
}
