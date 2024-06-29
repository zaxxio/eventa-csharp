using System;

public interface IQueryDispatcher
{
    R Dispatch<Q, R>(Q query, IResponseType<R> responseType);
}

public interface IResponseType<R>
{
    R Convert(object result);
}

public static class ResponseType
{
    public static IResponseType<T> InstanceOf<T>()
    {
        return new ResponseTypeImpl<T>((result) => (T)result);
    }

    private class ResponseTypeImpl<T> : IResponseType<T>
    {
        private Func<object, T> converter;

        public ResponseTypeImpl(Func<object, T> converter)
        {
            this.converter = converter;
        }

        public T Convert(object result)
        {
            return converter(result);
        }
    }
}

public class QueryDispatcher : IQueryDispatcher
{
    public R Dispatch<Q, R>(Q query, IResponseType<R> responseType)
    {
        object result = ExecuteQuery(query);
        return responseType.Convert(result);
    }

    private object ExecuteQuery<Q>(Q query)
    {
        return null;
    }
}