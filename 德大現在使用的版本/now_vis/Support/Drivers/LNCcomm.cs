using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace LNCcomm
{
    class CLNCc
    {
        public const int LNC_MAX_CONNECT_NUM = 1;

        //communication status, lnc_get_status(commSts);        //取得控制器的狀態
        public const int LNC_COMM_STATE_DISCONNECT = 0;
        public const int LNC_COMM_STATE_CONNECTING = 1;
        public const int LNC_COMM_STATE_FAIL = 2;
        public const int LNC_COMM_STATE_OK = 3;
        public const int LNC_COMM_STATE_NORESPONSE = 4;

        //sensor status, lnc_get_status(sensorSts)              //取得感測器的狀態
        public const int LNC_SENSOR_OFFLINE = 0;
        public const int LNC_SENSOR_ONLINE = 1;
        public const int LNC_SENSOR_BUFFER_OVERFLOW = 2;

        //select the frequency domain data of the axis, lnc_svi_set_fd_axis(axis), default: LNC_FD_SEL_X
        public const int LNC_FD_SEL_X = 0;    //設定頻域資料更新的軸(目前同時只會更新一個軸)   //X軸
        public const int LNC_FD_SEL_Y = 1;    //設定頻域資料更新的軸(目前同時只會更新一個軸)   //Y軸
        public const int LNC_FD_SEL_Z = 2;    //設定頻域資料更新的軸(目前同時只會更新一個軸)   //Z軸
        public const int LNC_FD_SEL_VECTOR = 3;    //設定頻域資料更新的軸(目前同時只會更新一個軸)   //V軸

        public const int LNC_FD_DATA_LENGTH_1D66K = 830;  //量測頻寬
        public const int LNC_FD_DATA_LENGTH_6D66K = 3330;

        // --- define function return code
        public const int LNC_ERR_NO_ERROR = (0);
        public const int LNC_ERR_FAILED = (-1);
        public const int LNC_ERR_WRONG_PARAM = (-2);
        public const int LNC_ERR_INCORRECT_NID = (-3);
        public const int LNC_ERR_TIMEOUT = (-4);
        public const int LNC_ERR_NOT_CONNECTED = (-5);
        public const int LNC_ERR_CMD_QUEUE_FULL = (-6);
        public const int LNC_ERR_NO_NEW_DATA = (-7);
        public const int LNC_ERR_SENSOR_OFFLINE = (-8);


        [DllImport("LNCcomm.dll", EntryPoint = "lnc_get_controller_cnt")]
        public static extern short lnc_get_controller_cnt(ref int existCnt);

        [DllImport("LNCcomm.dll", EntryPoint = "lnc_get_controller_info")]
        public static extern short lnc_get_controller_info(ushort index, [MarshalAsAttribute(UnmanagedType.LPStr)] StringBuilder name, [MarshalAsAttribute(UnmanagedType.LPStr)] StringBuilder ip);

        [DllImport("LNCcomm.dll", EntryPoint = "lnc_connect")]
        public static extern short lnc_connect(ushort nodeID, [MarshalAsAttribute(UnmanagedType.LPStr)] string IP, ushort timeout);

        [DllImport("LNCcomm.dll", EntryPoint = "lnc_disconnect")]
        public static extern short lnc_disconnect(ushort nodeID);

        [DllImport("LNCcomm.dll", EntryPoint = "lnc_get_status")]
        public static extern short lnc_get_status(ushort nodeID, ref byte commSts, ref byte sensorSts, ref int watchdogCnt);

        [DllImport("LNCcomm.dll", EntryPoint = "lnc_svi_set_zero")]
        public static extern short lnc_svi_set_zero(ushort nodeID);

        [DllImport("LNCcomm.dll", EntryPoint = "lnc_svi_clear_zero")]
        public static extern short lnc_svi_clear_zero(ushort nodeID);

        [DllImport("LNCcomm.dll", EntryPoint = "lnc_svi_enable")]
        public static extern short lnc_svi_enable(ushort nodeID, byte enable);

        [DllImport("LNCcomm.dll", EntryPoint = "lnc_svi_get_td_data_length")]
        public static extern short lnc_svi_get_td_data_length(ushort nodeID, ref uint length);

        [DllImport("LNCcomm.dll", EntryPoint = "lnc_svi_get_td_data")]
        public static extern short lnc_svi_get_td_data(ushort nodeID, uint length, ref short arrData, ref uint getNum);

        [DllImport("LNCcomm.dll", EntryPoint = "lnc_svi_set_fd_axis")]
        public static extern short lnc_svi_set_fd_axis(ushort nodeID, byte axis);

        [DllImport("LNCcomm.dll", EntryPoint = "lnc_svi_get_fd_data")]
        public static extern short lnc_svi_get_fd_data(ushort nodeID, ref float arrData);

        [DllImport("LNCcomm.dll", EntryPoint = "lnc_svi_get_cmd_error_cnt")]
        public static extern short lnc_svi_get_cmd_error_cnt(ushort nodeID, ref int errorCnt);

        [DllImport("LNCcomm.dll", EntryPoint = "lnc_svi_reset_cmd_error_cnt")]
        public static extern short lnc_svi_reset_cmd_error_cnt(ushort nodeID);
    }
}
