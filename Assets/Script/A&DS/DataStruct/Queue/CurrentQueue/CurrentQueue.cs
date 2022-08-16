namespace DataStruct.Queue.CurrentQueue
{
    public class CurrentQueue
    {
        ///参考c#自带的CurrentQueue
        ///其保证了数据入队出队的安全性，不会出现数据重复的情况
        ///高并发下数据虽然时乱的，但是不会重复
        ///相较与阻塞队列更加高效
        ///
        /// 核心：主要通过自旋和数值的原子性操作保证了并发的安全行，乐观锁
    }
}