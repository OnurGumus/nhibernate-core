#if NET_4_5
using System;
using NHibernate.Engine;
using NHibernate.Persister.Entity;
using System.Threading.Tasks;

namespace NHibernate.Proxy
{
	[System.CodeDom.Compiler.GeneratedCode("AsyncGenerator", "1.0.0")]
	public abstract partial class AbstractLazyInitializer : ILazyInitializer
	{
		/// <inheritdoc/>
		public async Task SetSessionAsync(ISessionImplementor s)
		{
			if (s != _session)
			{
				// check for s == null first, since it is least expensive
				if (s == null)
				{
					UnsetSession();
				}
				else if (IsConnectedToSession)
				{
					//TODO: perhaps this should be some other RuntimeException...
					throw new HibernateException("illegally attempted to associate a proxy with two open Sessions");
				}
				else
				{
					// s != null
					_session = s;
					if (readOnlyBeforeAttachedToSession == null)
					{
						// use the default read-only/modifiable setting
						IEntityPersister persister = s.Factory.GetEntityPersister(_entityName);
						await (SetReadOnlyAsync(s.PersistenceContext.DefaultReadOnly || !persister.IsMutable));
					}
					else
					{
						await (SetReadOnlyAsync(readOnlyBeforeAttachedToSession.Value));
						readOnlyBeforeAttachedToSession = null;
					}
				}
			}
		}

		/// <summary>
		/// Perform an ImmediateLoad of the actual object for the Proxy.
		/// </summary>
		/// <exception cref = "HibernateException">
		/// Thrown when the Proxy has no Session or the Session is closed or disconnected.
		/// </exception>
		public virtual async Task InitializeAsync()
		{
			if (!initialized)
			{
				if (_session == null)
				{
					throw new LazyInitializationException(_entityName, _id, "Could not initialize proxy - no Session.");
				}
				else if (!_session.IsOpen)
				{
					throw new LazyInitializationException(_entityName, _id, "Could not initialize proxy - the owning Session was closed.");
				}
				else if (!_session.IsConnected)
				{
					throw new LazyInitializationException(_entityName, _id, "Could not initialize proxy - the owning Session is disconnected.");
				}
				else
				{
					_target = await (_session.ImmediateLoadAsync(_entityName, _id));
					initialized = true;
					CheckTargetState();
				}
			}
			else
			{
				CheckTargetState();
			}
		}

		/// <summary>
		/// Return the Underlying Persistent Object, initializing if necessary.
		/// </summary>
		/// <returns>The Persistent Object this proxy is Proxying.</returns>
		public async Task<object> GetImplementationAsync()
		{
			await (InitializeAsync());
			return _target;
		}

		private async Task SetReadOnlyAsync(bool readOnly)
		{
			IEntityPersister persister = _session.Factory.GetEntityPersister(_entityName);
			if (!persister.IsMutable && !readOnly)
			{
				throw new InvalidOperationException("cannot make proxies for immutable entities modifiable");
			}

			this.readOnly = readOnly;
			if (initialized)
			{
				EntityKey key = GenerateEntityKeyOrNull(_id, _session, _entityName);
				if (key != null && _session.PersistenceContext.ContainsEntity(key))
				{
					await (_session.PersistenceContext.SetReadOnlyAsync(_target, readOnly));
				}
			}
		}
	}
}
#endif
