namespace LTs.Utils.test.Infrastructure;

internal static class AsyncEnumerableHelper
{
    public static async IAsyncEnumerable<T> GetAsyncEnumerable<T>( params T[] items )
    {
        foreach( var item in items )
        {
            await Task.Delay( 1 ); // simulate async work

            yield return item;
        }
    }
}