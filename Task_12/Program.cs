int orders = 0;
Semaphore semaphore = new Semaphore(2, 2);
ManualResetEvent Event = new ManualResetEvent(false);

Thread waiterThread = new Thread(() =>
{
    Event.WaitOne();
    Console.WriteLine("All orders have been processed!");
});
waiterThread.Start();

void AddOrders()
{
    semaphore.WaitOne();
    try
    {
        for (int i = 0; i < 100; i++)
        {
            Interlocked.Increment(ref orders);
        }
    }
    finally
    {
        semaphore.Release();
    }
}

Thread[] threads = new Thread[5];

for (int i = 0; i < 5; i++)
{
    threads[i] = new Thread(AddOrders);
    threads[i].Start();
}

foreach (var t in threads)
{
    t.Join();
}

Event.Set();
waiterThread.Join();

Console.WriteLine(orders);