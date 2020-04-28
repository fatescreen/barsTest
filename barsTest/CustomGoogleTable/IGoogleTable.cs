
namespace barsTest.CustomGoogleTable
{
    interface IGoogleTable
    {
        public void foreverUpdateStart(long intervalTimeInSec);
        public void foreverUpdateStop();
    }
}
