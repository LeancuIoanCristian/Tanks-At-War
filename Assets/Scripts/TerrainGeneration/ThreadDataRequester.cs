using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

namespace Terrain_Generation
{
    class ThreadDataRequester:MonoBehaviour
    {
		static ThreadDataRequester instance;
		Queue<ThreadInfo> data_queue = new Queue<ThreadInfo>();

		void Awake()
		{
			instance = FindObjectOfType<ThreadDataRequester>();
		}

		public static void RequestData(Func<object> generate_data, Action<object> callback)
		{
			ThreadStart thread_start = delegate {
				instance.DataThread(generate_data, callback);
			};

			new Thread(thread_start).Start();
		}

		void DataThread(Func<object> generate_data, Action<object> callback)
		{
			object data = generate_data();
			lock (data_queue)
			{
				data_queue.Enqueue(new ThreadInfo(callback, data));
			}
		}


		void Update()
		{
			if (data_queue.Count > 0)
			{
				for (int i = 0; i < data_queue.Count; i++)
				{
					ThreadInfo thread_info = data_queue.Dequeue();
					thread_info.callback(thread_info.parameter);
				}
			}
		}

		struct ThreadInfo
		{
			public readonly Action<object> callback;
			public readonly object parameter;

			public ThreadInfo(Action<object> callback, object parameter)
			{
				this.callback = callback;
				this.parameter = parameter;
			}

		}
	}
}
