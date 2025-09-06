using Core.Utilities.Results;
using IKitaplik.DataAccess.UnitOfWork;

public static class HandleWithTransactionHelper
{
    public static async Task<IResult> Handling(Func<Task<IResult>> operation, IUnitOfWork _unitOfWork)
    {

        try
        {
            _unitOfWork.BeginTransaction();
            var result = await operation();
            if (!result.Success)
            {
                _unitOfWork.Rollback();
                return result;
            }
            _unitOfWork.Commit();
            return result;
        }
        catch (Exception ex)
        {
            _unitOfWork.Rollback();
            return new ErrorResult("İşlem sırasında hata oluştu: " + ex.Message);
        }

    }

    public static async Task<IDataResult<T>> Handling<T>(Func<Task<IDataResult<T>>> operation, IUnitOfWork _unitOfWork) where T : new()
    {

        try
        {
            _unitOfWork.BeginTransaction();
            var result = await operation();
            if (!result.Success)
            {
                _unitOfWork.Rollback();
                return result;
            }
            _unitOfWork.Commit();
            return result;
        }
        catch (Exception ex)
        {
            _unitOfWork.Rollback();
            return new ErrorDataResult<T>(new T(), "İşlem sırasında hata oluştu: " + ex.Message);
        }
    }
}
