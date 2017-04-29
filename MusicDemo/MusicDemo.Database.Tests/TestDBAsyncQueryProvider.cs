using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace MusicDemo.Database.Tests
{
	/// <summary>
	/// Implementation sourced from https://msdn.microsoft.com/en-us/library/dn314429(v=vs.113).aspx
	/// </summary>
	internal class TestDBAsyncQueryProvider<TEntity> : IDbAsyncQueryProvider
	{
		private readonly IQueryProvider _inner;

		internal TestDBAsyncQueryProvider(IQueryProvider inner)
		{
			_inner = inner;
		}

		public IQueryable CreateQuery(Expression expression)
		{
			return new TestDBAsyncEnumerable<TEntity>(expression);
		}

		public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
		{
			return new TestDBAsyncEnumerable<TElement>(expression);
		}

		public object Execute(Expression expression)
		{
			return _inner.Execute(expression);
		}

		public TResult Execute<TResult>(Expression expression)
		{
			return _inner.Execute<TResult>(expression);
		}

		public Task<object> ExecuteAsync(Expression expression, CancellationToken cancellationToken)
		{
			return Task.FromResult(Execute(expression));
		}

		public Task<TResult> ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken)
		{
			return Task.FromResult(Execute<TResult>(expression));
		}
	}

	internal class TestDBAsyncEnumerable<T> : EnumerableQuery<T>, IDbAsyncEnumerable<T>, IQueryable<T>
	{
		public TestDBAsyncEnumerable(IEnumerable<T> enumerable)
			: base(enumerable)
		{ }

		public TestDBAsyncEnumerable(Expression expression)
			: base(expression)
		{ }

		public IDbAsyncEnumerator<T> GetAsyncEnumerator()
		{
			return new TestDBAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());
		}

		IDbAsyncEnumerator IDbAsyncEnumerable.GetAsyncEnumerator()
		{
			return GetAsyncEnumerator();
		}

		IQueryProvider IQueryable.Provider
		{
			get { return new TestDBAsyncQueryProvider<T>(this); }
		}
	}

	internal class TestDBAsyncEnumerator<T> : IDbAsyncEnumerator<T>
	{
		private readonly IEnumerator<T> _inner;

		public TestDBAsyncEnumerator(IEnumerator<T> inner)
		{
			_inner = inner;
		}

		public void Dispose()
		{
			_inner.Dispose();
		}

		public Task<bool> MoveNextAsync(CancellationToken cancellationToken)
		{
			return Task.FromResult(_inner.MoveNext());
		}

		public T Current
		{
			get { return _inner.Current; }
		}

		object IDbAsyncEnumerator.Current
		{
			get { return Current; }
		}
	}
}
