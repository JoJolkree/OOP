using System;

namespace MyPhotoshop
{

	
	public interface IFilter
	{
        /// <summary>
        /// ���� ����� ������ ���������� �������� ����������, ������� ���������� � NumericUpDown-��������
        /// ����� �� �������� ������ �������
        /// </summary>
        /// <returns></returns>
  	    ParameterInfo[] GetParameters();
        /// <summary>
        /// ���� ����� ��������� ����������, ������� ���� ������������, � ��������� �������� ���� ����������
        /// ����� ������� values � �������� ����� ����� �������, ������������� ������� GetParameters
        /// </summary>
        /// <param name="original"></param>
        /// <param name="values"></param>
        /// <returns></returns>
		Photo Process(Photo original, double[] values);
	}
}

