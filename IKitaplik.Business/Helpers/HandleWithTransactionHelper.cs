using Core.Utilities.Results;
using IKitaplik.DataAccess.UnitOfWork;

public static class HandleWithTransactionHelper
{
    public static IResult Handling(Func<IResult> operation, IUnitOfWork _unitOfWork)
    {
        try
        {
            _unitOfWork.BeginTransaction();
            var result = operation();
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

    public static IDataResult<T> Handling<T>(Func<IDataResult<T>> operation, IUnitOfWork _unitOfWork) where T : new()
    {
        try
        {
            _unitOfWork.BeginTransaction();
            var result = operation();
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
