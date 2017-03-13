namespace MyPhotoshop
{
    public abstract class ParametrizedFilter<T> : IFilter
        where T : IParameters, new()
    {
        public ParameterInfo[] GetParameters()
        {
            return new T().GetDescription();
        }

        public Photo Process(Photo original, double[] values)
        {
            var parameters = new T();
            parameters.SetValues(values);
            return Process(original, parameters);
        }

        public abstract Photo Process(Photo original, T parameters);
    }
}