using System.Collections.Concurrent;

namespace kisa_gcs_system.Models.Helper;

// 제한된 크기의 큐 클래스
public class FixedSizedQueue<T>
{
    // ConcurrentQueue<T>는 다중 스레드 환경에서 안전하게 사용할 수 있는 큐를 나타내는 .NET의 클래스이다.
    public ConcurrentQueue<T> q = new();
    // lock문을 사용하여 큐에 대한 동기화를 보장하기 위한 개체이다. 큐에 요소를 추가하고 제거할 때 동시에 여러 스레드에서 호출될 수 있으므로 _lockObject를 사용하여 스레드 간에 상호 배제를 수행한다.
    private object _lockObject = new();

    // 생성자
    public FixedSizedQueue(int limit)
    {
        Limit = limit;
    }
    
    // 필드
    private int Limit { get; }

    // 메소드
    // 큐에 새로운 요소를 추가하는 메서드
    public void Enqueue(T obj)
    {
        // ConcurrentQueue<T>의 Enqueue 메서드를 사용하여 요소를 큐에 추가
        q.Enqueue(obj);
        // lock문을 사용하여 큐의 크기를 제한하고 초과하는 요소를 제거
        lock (_lockObject)
        {
            // TryDequeue를 사용하여 큐에서 요소를 제거 큐가 비어 있을 때까지 제한 크기를 유지
            while (q.Count > Limit && q.TryDequeue(out T overflow));
        }
    }
}