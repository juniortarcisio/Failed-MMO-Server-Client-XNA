using System;
using System.Collections.Generic;
using System.Text;

public class NetMsg
{
    byte[] m_buff;
    int readPos;


    //Constructors
    public NetMsg(byte[] buff)
    {
        m_buff = buff;
        readPos = 0;
    }

    public NetMsg()
    {
        m_buff = new Byte[16384];
        readPos = 0;
    }

    //Properties
    public byte[] GetBuff
    {
        get { return m_buff; }
    }

    public int Length
    {
        get { return readPos; }
    }

    //Add Methods
    public void AddByte(byte b)
    {
        m_buff[readPos++] = b;
    }

    public void AddString(string s)
    {
        Byte[] m_bStr = new UTF8Encoding().GetBytes(s);

        m_buff[readPos++] = (byte)s.Length;

        Buffer.BlockCopy(m_bStr, 0, this.m_buff, readPos, (byte)s.Length);
        this.readPos += s.Length;
    }

    //Get Methods
    public byte GetByte()
    {
        return this.m_buff[readPos++];
    }

    public string GetString()
    {
        int strLen = this.m_buff[readPos++];
        Byte[] m_bStr = new Byte[strLen];

        Buffer.BlockCopy(this.m_buff, readPos, m_bStr, 0, strLen);
        this.readPos += strLen;

        return new ASCIIEncoding().GetString(m_bStr);
    }

}
