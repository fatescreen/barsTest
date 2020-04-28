
namespace barsTest.CustomGoogleTable
{
    /// <summary>
    /// Interface for GoogleTable class
    /// </summary>
    interface IGoogleTable
    {
        public void foreverUpdateStart(long intervalTimeInSec);
        public void foreverUpdateStop();
    }
}
