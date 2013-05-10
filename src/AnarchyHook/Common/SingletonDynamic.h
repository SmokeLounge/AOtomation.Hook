#pragma once

namespace AnarchyHook {
namespace Common {

// Policy based singleton template
// http://stackoverflow.com/questions/3444445/c-singleton-template-class

/**
 * This is the default ConcurrencyPolicy implementation for the SingletonDynamic class. This
 * implementation does not provide thread-safety and is merely a placeholder. Classes deriving from
 * SingletonDynamic must provide alternate ConcurrencyPolicy implementations if thread-safety is
 * desired.
 */
struct DefaultSingletonConcurrencyPolicy {
    /**
     * Placeholder function for locking a mutex, thereby preventing access to other threads. This
     * default implementation does not perform any function, the derived class must provide an
     * alternate implementation if this functionality is desired.
     */
    static void lock_mutex() {
        /* default implementation does nothing */
        return;
    }

    /**
     * Placeholder function for unlocking a mutex, thereby allowing access to other threads. This
     * default implementation does not perform any function, the derived class must provide an
     * alternate implementation if this functionality is desired.
     */
    static void unlock_mutex() {
        /* default implementation does nothing */
        return;
    }

    /**
     * Placeholder function for executing a memory barrier instruction, thereby preventing the
     * compiler from reordering read and writes across this boundary. This default implementation does
     * not perform any function, the derived class must provide an alternate implementation if this
     * functionality is desired.
     */
    static void memory_barrier() {
        /* default implementation does nothing */
        return;
    }
};


/**
 * @brief
 *    Singleton design pattern implementation using a dynamically allocated singleton instance.
 *
 * The SingletonDynamic class is intended for use as a base for classes implementing the Singleton
 * design pattern and that dynamic allocation of the singleton object. The default implementation
 * is not thread-safe; however, the class uses a policy-based design pattern that allows the derived
 * classes to achieve threaad-safety by providing an alternate implementation of the
 * ConcurrencyPolicy.
 *
 * @tparam T
 *    The type name of the derived (singleton) class
 * @tparam ConcurrencyPolicy
 *    The policy implementation for providing thread-safety
 *
 * @note The derived class must have a no-throw default constructor and a no-throw destructor.
 * @note The derived class must list this class as a friend, since, by necessity, the derived class'
 *       constructors must be protected / private.
 */
template< typename T, typename ConcurrencyPolicy = DefaultSingletonConcurrencyPolicy >
class SingletonDynamic : public ConcurrencyPolicy {
public:
    /**
     * Factory function for vending mutable references to the sole instance of the singleton object.
     *
     * @return A mutable reference to the one and only instance of the singleton object.
     */
    static T &instance() {
        return *SingletonDynamic< T, ConcurrencyPolicy >::get_instance();
    }


    /**
     * Factory function for vending constant references to the sole instance of the singleton object.
     *
     * @return A constant reference to the one and only instance of the singleton object.
     */
    static const T &const_instance() {
        return *SingletonDynamic< T, ConcurrencyPolicy >::get_instance();
    }

protected:
    /** Default constructor */
    SingletonDynamic() {}

    /** Destructor */
    virtual ~SingletonDynamic() {
        delete SingletonDynamic< T, ConcurrencyPolicy >::pInstance_;
    }

private:
    /** The sole instance of the singleton object */
    static T *pInstance_;

    /** Flag indicating whether the singleton object has been created */
    static volatile bool flag_;

    /** Private copy constructor to prevent copy construction */
    SingletonDynamic( SingletonDynamic const & );

    /** Private operator to prevent assignment */
    SingletonDynamic &operator=( SingletonDynamic const & );


    /**
     * Fetches a pointer to the singleton object, after creating it if necessary
     *
     * @return A pointer to the one and only instance of the singleton object.
     */
    static T *get_instance() {
        if ( SingletonDynamic< T, ConcurrencyPolicy >::flag_ == false ) {
            /* acquire lock */
            ConcurrencyPolicy::lock_mutex();

            /* create the singleton object if this is the first time */
            if ( SingletonDynamic< T, ConcurrencyPolicy >::pInstance_ == NULL ) {
                pInstance_ = new T();
            }

            /* release lock */
            ConcurrencyPolicy::unlock_mutex();

            /* enforce all prior I/O to be completed */
            ConcurrencyPolicy::memory_barrier();

            /* set flag to indicate singleton has been created */
            SingletonDynamic< T, ConcurrencyPolicy >::flag_ = true;

            return SingletonDynamic< T, ConcurrencyPolicy >::pInstance_;
        } else {
            /* enforce all prior I/O to be completed */
            ConcurrencyPolicy::memory_barrier();

            return SingletonDynamic< T, ConcurrencyPolicy >::pInstance_;
        }
    }
};

/* Initialize the singleton instance pointer */
template< typename T, typename ConcurrencyPolicy >
T *SingletonDynamic< T , ConcurrencyPolicy >::pInstance_        = NULL;

/* Initialize the singleton flag */
template< typename T, typename ConcurrencyPolicy >
volatile bool SingletonDynamic< T , ConcurrencyPolicy >::flag_  = false;

}
}
