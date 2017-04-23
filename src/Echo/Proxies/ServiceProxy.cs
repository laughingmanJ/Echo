using Echo.Interfaces;
using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Echo.Proxies
{
    /// <summary>
    /// Base class that contains common operations shared by channel proxies.
    /// </summary>
    /// <typeparam name="T">Service contract.</typeparam>
    /// <typeparam name="TR">Channel factory type</typeparam>
    class ServiceProxy<T, TR> : IServiceProxy<T>
        where T : class
        where TR : ChannelFactory<T>
    {

        /// <summary>
        /// Channel for conducting service calls.
        /// </summary>
        public T Channel { get; private set; }


        /// <summary>
        /// Channel factory for creating service channels.
        /// </summary>
        private TR _channelFactory;


        public ServiceProxy(TR channelFactory)
        {
            _channelFactory = channelFactory;
            InitializeChannel();
        }

        /// <summary>
        /// Initializes proxy's channel from the channel factory and assigns event handlers. 
        /// </summary>
        public void InitializeChannel()
        {
            // Create channel factory from child's class implemented method.
            Channel = _channelFactory.CreateChannel();

            // Assign internal events to monitor failure and channel opening.
            ((IChannel)Channel).Faulted += ChannelFaulted;
        }

        /// <summary>
        /// Closes any open channels.
        /// </summary>
        public virtual void Dispose()
        {
            CloseChannel(Channel);
            // Shutdown the channel and close the factory.
            if (CommunicationState.Closed != _channelFactory.State)
                _channelFactory.Close();
        }


        #region Event Handlers


        /// <summary>
        /// Occurs when the channel enters a faulted state and then attempts to reinitialize
        ///     the channel and factory for possible reconnection
        /// </summary>
        /// <param name="sender">Channel.</param>
        /// <param name="e">Empty event args.</param>
        protected virtual void ChannelFaulted(object sender, EventArgs e)
        {
            // Shutdown the faulted channel gracefully.
            ((IChannel)Channel).Abort();
            ((IChannel)Channel).Close();

            // Shutdown the channel factory.
            _channelFactory.Close();

        }

        #endregion

        /// <summary>
        /// Closes channel properly.
        /// </summary>
        /// <param name="channel">Channel to close</param>
        private static void CloseChannel(T channel)
        {
            if (null == channel) return;

            var clientChannel = (IClientChannel)channel;

            // When a channel is in a faulted state, it can only be aborted which closes it
            if (CommunicationState.Faulted == clientChannel.State)
                clientChannel.Abort();
            else if (CommunicationState.Closed != clientChannel.State)
                clientChannel.Close();
        }
    }
}
