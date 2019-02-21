using MissionPlanner.ArduPilot;
using MissionPlanner.Utilities;
using SKYROVER.GCS.DeskTop.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SKYROVER.GCS.DeskTop.MessagePanel
{
    public partial class ConfigParameters : Form
    {

        // from http://stackoverflow.com/questions/2512781/winforms-big-paragraph-tooltip/2512895#2512895
        private const int maximumSingleLineTooltipLength = 50;
        private static readonly Hashtable tooltips = new Hashtable();
        private readonly Hashtable changes = new Hashtable();
        internal bool startup = true;
        public ConfigParameters()
        {
            InitializeComponent();

          
        }
        /// <summary>
        /// 
        /// </summary>
        public void Activate()
        {
            if (!MainUI.comPort.BaseStream.IsOpen)
            {
                Enabled = false;
                return;
            }
            if (MainUI.comPort.MAV.cs.firmware == Firmwares.ArduPlane)
            {
                Enabled = true;
            }
            else
            {
                Enabled = false;
                return;
            }


            startup = true;
            #region 原始参数 已经不用

            //THR_SLEWRATE.setup(0, 0, 1, 0, "THR_SLEWRATE", MainUI.comPort.MAV.param);
            //THR_MAX.setup(0, 0, 1, 0, "THR_MAX", MainUI.comPort.MAV.param);
            //THR_MIN.setup(0, 0, 1, 0, "THR_MIN", MainUI.comPort.MAV.param);
            //TRIM_THROTTLE.setup(0, 0, 1, 0, "TRIM_THROTTLE", MainUI.comPort.MAV.param);

            //ARSPD_RATIO.setup(0, 0, 1, 0, "ARSPD_RATIO", MainUI.comPort.MAV.param);
            //ARSPD_FBW_MAX.setup(0, 0, 1, 0, "ARSPD_FBW_MAX", MainUI.comPort.MAV.param);
            //ARSPD_FBW_MIN.setup(0, 0, 1, 0, "ARSPD_FBW_MIN", MainUI.comPort.MAV.param);
            //TRIM_ARSPD_CM.setup(0, 0, 100, 0, "TRIM_ARSPD_CM", MainUI.comPort.MAV.param);

            //LIM_PITCH_MIN.setup(0, 0, 100, 0, "LIM_PITCH_MIN", MainUI.comPort.MAV.param);
            //LIM_PITCH_MAX.setup(0, 0, 100, 0, "LIM_PITCH_MAX", MainUI.comPort.MAV.param);
            //LIM_ROLL_CD.setup(0, 0, 100, 0, "LIM_ROLL_CD", MainUI.comPort.MAV.param);

            //KFF_PTCH2THR.setup(0, 0, 1, 0, "KFF_PTCH2THR", MainUI.comPort.MAV.param);
            //KFF_RDDRMIX.setup(0, 0, 1, 0, "KFF_RDDRMIX", MainUI.comPort.MAV.param);

            //ENRGY2THR_IMAX.setup(0, 0, 100, 0, "ENRGY2THR_IMAX", MainUI.comPort.MAV.param);
            //ENRGY2THR_D.setup(0, 0, 1, 0, "ENRGY2THR_D", MainUI.comPort.MAV.param);
            //ENRGY2THR_I.setup(0, 0, 1, 0, "ENRGY2THR_I", MainUI.comPort.MAV.param);
            //ENRGY2THR_P.setup(0, 0, 1, 0, "ENRGY2THR_P", MainUI.comPort.MAV.param);

            //ALT2PTCH_IMAX.setup(0, 0, 100, 0, "ALT2PTCH_IMAX", MainUI.comPort.MAV.param);
            //ALT2PTCH_D.setup(0, 0, 1, 0, "ALT2PTCH_D", MainUI.comPort.MAV.param);
            //ALT2PTCH_I.setup(0, 0, 1, 0, "ALT2PTCH_I", MainUI.comPort.MAV.param);
            //ALT2PTCH_P.setup(0, 0, 1, 0, "ALT2PTCH_P", MainUI.comPort.MAV.param);

            //ARSP2PTCH_IMAX.setup(0, 0, 100, 0, "ARSP2PTCH_IMAX", MainUI.comPort.MAV.param);
            //ARSP2PTCH_D.setup(0, 0, 1, 0, "ARSP2PTCH_D", MainUI.comPort.MAV.param);
            //ARSP2PTCH_I.setup(0, 0, 1, 0, "ARSP2PTCH_I", MainUI.comPort.MAV.param);
            //ARSP2PTCH_P.setup(0, 0, 1, 0, "ARSP2PTCH_P", MainUI.comPort.MAV.param);

            //YAW2SRV_IMAX.setup(0, 0, 100, 0, "YAW2SRV_IMAX", MainUI.comPort.MAV.param);
            //YAW2SRV_DAMP.setup(0, 0, 1, 0, "YAW2SRV_DAMP", MainUI.comPort.MAV.param);
            //YAW2SRV_INT.setup(0, 0, 1, 0, "YAW2SRV_INT", MainUI.comPort.MAV.param);
            //YAW2SRV_RLL.setup(0, 0, 1, 0, "YAW2SRV_RLL", MainUI.comPort.MAV.param);

            //PTCH2SRV_IMAX.setup(0, 0, 100, 0, "PTCH2SRV_IMAX", MainUI.comPort.MAV.param);
            //PTCH2SRV_D.setup(0, 0, 1, 0, "PTCH2SRV_D", MainUI.comPort.MAV.param);
            //PTCH2SRV_I.setup(0, 0, 1, 0, "PTCH2SRV_I", MainUI.comPort.MAV.param);
            //PTCH2SRV_P.setup(0, 0, 1, 0, "PTCH2SRV_P", MainUI.comPort.MAV.param);

            //RLL2SRV_IMAX.setup(0, 0, 100, 0, "RLL2SRV_IMAX", MainUI.comPort.MAV.param);
            //RLL2SRV_D.setup(0, 0, 1, 0, "RLL2SRV_D", MainUI.comPort.MAV.param);
            //RLL2SRV_I.setup(0, 0, 1, 0, "RLL2SRV_I", MainUI.comPort.MAV.param);
            //RLL2SRV_P.setup(0, 0, 1, 0, "RLL2SRV_P", MainUI.comPort.MAV.param);

            //NAVL1_DAMPING.setup(0, 0, 1, 0, "NAVL1_DAMPING", MainUI.comPort.MAV.param);
            //NAVL1_PERIOD.setup(0, 0, 1, 0, "NAVL1_PERIOD", MainUI.comPort.MAV.param);

            //TECS_SINK_MAX.setup(0, 0, 1, 0, "TECS_SINK_MAX", MainUI.comPort.MAV.param);
            //TECS_TIME_CONST.setup(0, 0, 1, 0, "TECS_TIME_CONST", MainUI.comPort.MAV.param);
            //TECS_PTCH_DAMP.setup(0, 0, 1, 0, "TECS_PTCH_DAMP", MainUI.comPort.MAV.param);
            //TECS_SINK_MIN.setup(0, 0, 1, 0, "TECS_SINK_MIN", MainUI.comPort.MAV.param);
            //TECS_CLMB_MAX.setup(0, 0, 1, 0, "TECS_CLMB_MAX", MainUI.comPort.MAV.param);
            #endregion

            //初始化VTOL参数
            InitParams();


            changes.Clear();

            // add tooltips to all controls
            addTooltips();

            //激活Q_TILT_MASK
            activeQ_TILT_MASK(true);


            startup = false;
        }
        /// <summary>
        /// 
        /// </summary>
        private void addTooltips() {

            foreach (Control control1 in Controls)
            {
                foreach (Control control2 in control1.Controls)
                {
                    if (control2 is MavlinkNumericUpDown)
                    {
                        var ParamName = ((MavlinkNumericUpDown)control2).ParamName;
                        toolTip1.SetToolTip(control2,
                            ParameterMetaDataRepository.GetParameterMetaData(ParamName,
                                ParameterMetaDataConstants.Description, MainUI.comPort.MAV.cs.firmware.ToString()));
                    }
                    else if (control2 is MavlinkComboBox)
                    {
                        var ParamName = ((MavlinkComboBox)control2).ParamName;
                        toolTip1.SetToolTip(control2,
                            ParameterMetaDataRepository.GetParameterMetaData(ParamName,
                                ParameterMetaDataConstants.Description, MainUI.comPort.MAV.cs.firmware.ToString()));
                    }
                    else if (control2 is MavlinkCheckBox)
                    {
                        var ParamName = ((MavlinkCheckBox)control2).ParamName;
                        toolTip1.SetToolTip(control2,
                            ParameterMetaDataRepository.GetParameterMetaData(ParamName,
                                ParameterMetaDataConstants.Description, MainUI.comPort.MAV.cs.firmware.ToString()));
                    }
                }
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="enabled"></param>
        private void activeQ_TILT_MASK(bool enabled)
        {
          
            Q_TILT_MASK_1.Enabled = enabled;
            Q_TILT_MASK_2.Enabled = enabled;
            Q_TILT_MASK_3.Enabled = enabled;
            Q_TILT_MASK_4.Enabled = enabled;
            Q_TILT_MASK_5.Enabled = enabled;
            Q_TILT_MASK_6.Enabled = enabled;
            Q_TILT_MASK_7.Enabled = enabled;
            Q_TILT_MASK_8.Enabled = enabled;


        }


        /// <summary>
        /// 初始化VTOL参数
        /// </summary>
        private void InitParams()
        {

            #region 飞行器结构
            Q_ENABLE.setup(1, 0, "Q_ENABLE", MainUI.comPort.MAV.param);
            Q_FRAME_CLASS.setup(ParameterMetaDataRepository.GetParameterOptionsInt("Q_FRAME_CLASS", MainUI.comPort.MAV.cs.firmware.ToString()).ToList(), "Q_FRAME_CLASS", MainUI.comPort.MAV.param);
            Q_FRAME_TYPE.setup(ParameterMetaDataRepository.GetParameterOptionsInt("Q_FRAME_TYPE", MainUI.comPort.MAV.cs.firmware.ToString()).ToList(), "Q_FRAME_TYPE", MainUI.comPort.MAV.param);
            #endregion

            #region 自稳Roll、PITCH、YAW
            Q_A_ANG_RLL_P.setup(0, 0, 100, 0, "Q_A_ANG_RLL_P", MainUI.comPort.MAV.param);
            Q_A_ANG_PIT_P.setup(0, 0, 100, 0, "Q_A_ANG_PIT_P", MainUI.comPort.MAV.param);
            Q_A_ANG_YAW_P.setup(0, 0, 100, 0, "Q_A_ANG_YAW_P", MainUI.comPort.MAV.param);
            #endregion

            #region  Roll 速率
            Q_A_RAT_RLL_P.setup(0, 0, 1, 0, "Q_A_RAT_RLL_P", MainUI.comPort.MAV.param);
            Q_A_RAT_RLL_I.setup(0, 0, 1, 0, "Q_A_RAT_RLL_I", MainUI.comPort.MAV.param);
            Q_A_RAT_RLL_D.setup(0, 0, 1, 0, "Q_A_RAT_RLL_D", MainUI.comPort.MAV.param);
            Q_A_RAT_RLL_IMAX.setup(0, 0, 100, 0, "Q_A_RAT_RLL_IMAX", MainUI.comPort.MAV.param);
            Q_A_RAT_RLL_FF.setup(0, 0, 100, 0, "Q_A_RAT_RLL_FF", MainUI.comPort.MAV.param);
            Q_A_RAT_RLL_FILT.setup(0, 0, 1, 0, "Q_A_RAT_RLL_FILT", MainUI.comPort.MAV.param);
            #endregion
            
            #region PITCH速率
            Q_A_RAT_PIT_P.setup(0, 0, 1, 0, "Q_A_RAT_PIT_P", MainUI.comPort.MAV.param);
            Q_A_RAT_PIT_I.setup(0, 0, 100, 0, "Q_A_RAT_PIT_I", MainUI.comPort.MAV.param);
            Q_A_RAT_PIT_D.setup(0, 0, 1, 0, "Q_A_RAT_PIT_D", MainUI.comPort.MAV.param);
            Q_A_RAT_PIT_IMAX.setup(0, 0, 1, 0, "Q_A_RAT_PIT_IMAX", MainUI.comPort.MAV.param);
            Q_A_RAT_PIT_FF.setup(0, 0, 1, 0, "Q_A_RAT_PIT_FF", MainUI.comPort.MAV.param);
            Q_A_RAT_PIT_FILT.setup(0, 0, 100, 0, "Q_A_RAT_PIT_FILT", MainUI.comPort.MAV.param);

            #endregion
            
            #region YAW 速率
            Q_A_RAT_YAW_P.setup(0, 0, 1, 0, "Q_A_RAT_YAW_P", MainUI.comPort.MAV.param);
            Q_A_RAT_YAW_I.setup(0, 0, 1, 0, "Q_A_RAT_YAW_I", MainUI.comPort.MAV.param);
            Q_A_RAT_YAW_D.setup(0, 0, 1, 0, "Q_A_RAT_YAW_D", MainUI.comPort.MAV.param);
            Q_A_RAT_YAW_IMAX.setup(0, 0, 100, 0, "Q_A_RAT_YAW_IMAX", MainUI.comPort.MAV.param);
            Q_A_RAT_YAW_FF.setup(0, 0, 1, 0, "Q_A_RAT_YAW_FF", MainUI.comPort.MAV.param);
            Q_A_RAT_YAW_FILT.setup(0, 0, 1, 0, "Q_A_RAT_YAW_FILT", MainUI.comPort.MAV.param);
            #endregion



            #region 旋翼/固定翼模式转换
            ARSPD_FBW_MIN_1.setup(0, 0, 1, 0, "ARSPD_FBW_MIN", MainUI.comPort.MAV.param);
            Q_TRANSITION_MS.setup(0, 0, 100, 0, "Q_TRANSITION_MS", MainUI.comPort.MAV.param);
            Q_ASSIST_SPEED.setup(0, 0, 1, 0, "Q_ASSIST_SPEED", MainUI.comPort.MAV.param);
            Q_ASSIST_ANGLE.setup(0, 0, 1, 0, "Q_ASSIST_ANGLE", MainUI.comPort.MAV.param);
            Q_M_HOVER_LEARN.setup(1, 0, "Q_M_HOVER_LEARN", MainUI.comPort.MAV.param);
            #endregion

            #region 航点导航
            Q_GUIDED_MODE.setup(1, 0, "Q_GUIDED_MODE", MainUI.comPort.MAV.param);
            Q_RTL_MODE.setup(1, 0, "Q_RTL_MODE", MainUI.comPort.MAV.param);
            Q_RTL_ALT.setup(0, 0, 1, 0, "Q_RTL_ALT", MainUI.comPort.MAV.param);
            Q_WP_SPEED.setup(0, 0, 1, 0, "Q_WP_SPEED", MainUI.comPort.MAV.param);
            Q_WP_RADIUS.setup(0, 0, 100, 0, "Q_WP_RADIUS", MainUI.comPort.MAV.param);
            Q_WP_SPEED_UP.setup(0, 0, 1, 0, "Q_WP_SPEED_UP", MainUI.comPort.MAV.param);
            Q_WP_SPEED_DN.setup(0, 0, 1, 0, "Q_WP_SPEED_DN", MainUI.comPort.MAV.param);
            Q_LAND_SPEED.setup(0, 0, 1, 0, "Q_LAND_SPEED", MainUI.comPort.MAV.param);
            Q_LAND_FINAL_ALT.setup(0, 0, 1, 0, "Q_LAND_FINAL_ALT", MainUI.comPort.MAV.param);
            #endregion


            #region 倾转垂起功能
            //确定控制哪几个倾转电机项
            setQ_TILT_MASK_Value((int)MainUI.comPort.MAV.param["Q_TILT_MASK"].Value);

            Q_TILT_TYPE.setup(
            ParameterMetaDataRepository.GetParameterOptionsInt("Q_TILT_TYPE", MainUI.comPort.MAV.cs.firmware.ToString())
                .ToList(), "Q_TILT_TYPE", MainUI.comPort.MAV.param);
            Q_TILT_RATE_UP.setup(0, 0, 1, 0, "Q_TILT_RATE_UP", MainUI.comPort.MAV.param);
            Q_TILT_RATE_DN.setup(0, 0, 1, 0, "Q_TILT_RATE_DN", MainUI.comPort.MAV.param);
            Q_TILT_MAX.setup(0, 0, 1, 0, "Q_TILT_MAX", MainUI.comPort.MAV.param);
            Q_TILT_YAW_ANGLE.setup(0, 0, 1, 0, "Q_TILT_YAW_ANGLE", MainUI.comPort.MAV.param);
            #endregion

            #region 尾座式垂起功能
            Q_TAILSIT_ANGLE.setup(0, 0, 1, 0, "Q_TAILSIT_ANGLE", MainUI.comPort.MAV.param);
            Q_TAILSIT_INPUT.setup(
            ParameterMetaDataRepository.GetParameterOptionsInt("Q_TAILSIT_INPUT", MainUI.comPort.MAV.cs.firmware.ToString())
               .ToList(), "Q_TAILSIT_INPUT", MainUI.comPort.MAV.param);
            //手动控制舵面
            Q_TAILSIT_MASK.setup("Q_TAILSIT_MASK", MainUI.comPort.MAV.param);
            Q_TAILSIT_MASK.myLabel1.Text = "手动控制舵面";
            Q_TAILSIT_MASKCH.setup(
           ParameterMetaDataRepository.GetParameterOptionsInt("Q_TAILSIT_MASKCH", MainUI.comPort.MAV.cs.firmware.ToString())
               .ToList(), "Q_TAILSIT_MASKCH", MainUI.comPort.MAV.param);
            #endregion
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private int getQ_TILT_MASK_Params()
        {
            if (startup) return 0;

            int Q_TILT_MASK_Value = 0;
            if (Q_TILT_MASK_1.Checked) Q_TILT_MASK_Value += 1;
          
            if (Q_TILT_MASK_2.Checked) Q_TILT_MASK_Value += 2;
         
            if (Q_TILT_MASK_3.Checked) Q_TILT_MASK_Value += 4;
            
            if (Q_TILT_MASK_4.Checked) Q_TILT_MASK_Value += 8;
          
            if (Q_TILT_MASK_5.Checked) Q_TILT_MASK_Value += 16;
           
            if (Q_TILT_MASK_6.Checked) Q_TILT_MASK_Value += 32;
          
            if (Q_TILT_MASK_7.Checked) Q_TILT_MASK_Value += 64;
            
            if (Q_TILT_MASK_8.Checked) Q_TILT_MASK_Value += 128;
          
            return Q_TILT_MASK_Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        private void setQ_TILT_MASK_Value(int value)
        {

            string temp = Convert.ToString(value, 2);

            int delt = 8 - temp.Length;
            if (delt != 0)
            {
                for (int i = 0; i < delt; i++)
                {
                    temp = 0 + temp;
                }
            }
            if (temp.Length == 8)
            {
                var tempArry = temp.ToArray();
                Q_TILT_MASK_1.Checked = (tempArry[7] == '1') ? true : false;
                Q_TILT_MASK_2.Checked = (tempArry[6] == '1') ? true : false;
                Q_TILT_MASK_3.Checked = (tempArry[5] == '1') ? true : false;
                Q_TILT_MASK_4.Checked = (tempArry[4] == '1') ? true : false;

                Q_TILT_MASK_5.Checked = (tempArry[3] == '1') ? true : false;
                Q_TILT_MASK_6.Checked = (tempArry[2] == '1') ? true : false;
                Q_TILT_MASK_7.Checked = (tempArry[1] == '1') ? true : false;
                Q_TILT_MASK_8.Checked = (tempArry[0] == '1') ? true : false;
            }

        }

        /// <summary>
        /// 转换为二进制
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToErJin(int value)
        {
            int temp = 0;
            int shang = 1;
            int yushu;
            while (shang != 0)
            {
                shang = (int)value / 2;
                yushu = value % 2;
                value = shang;
                temp += yushu;
                if (shang != 0)
                {
                    temp = temp * 10;
                }
            }
            //最后将 temp 倒序
            string tempStr = temp.ToString();
            int tempLength = tempStr.Length;
            string resultStr = string.Empty;
            for (int i = 0; i < tempLength; i++)
            {
                resultStr = tempStr[i] + resultStr;
            }
            return int.Parse(resultStr);
        }



        /// <summary>
        ///设置激活
        /// </summary>
        /// <param name="enable"></param>
        private void SetQ_ParametersEnable(bool enable)
        {

            #region 飞行器结构
            //  Q_ENABLE.Enabled = enable;
            Q_FRAME_CLASS.Enabled = enable;//(ParameterMetaDataRepository.GetParameterOptionsInt("Q_FRAME_CLASS", MainUI.comPort.MAV.cs.firmware.ToString()).ToList(), "Q_FRAME_CLASS", MainUI.comPort.MAV.param);
            Q_FRAME_TYPE.Enabled = enable;//(ParameterMetaDataRepository.GetParameterOptionsInt("Q_FRAME_TYPE", MainUI.comPort.MAV.cs.firmware.ToString()).ToList(), "Q_FRAME_TYPE", MainUI.comPort.MAV.param);
            #endregion

            #region 自稳Roll、PITCH、YAW
            Q_A_ANG_RLL_P.Enabled = enable;//(0, 0, 1, 0, "Q_A_ANG_RLL_P", MainUI.comPort.MAV.param);
            Q_A_ANG_PIT_P.Enabled = enable;//(0, 0, 1, 0, "Q_A_ANG_PIT_P", MainUI.comPort.MAV.param);
            Q_A_ANG_YAW_P.Enabled = enable;//(0, 0, 1, 0, "Q_A_ANG_YAW_P", MainUI.comPort.MAV.param);
            #endregion

            #region  Roll 速率
            Q_A_RAT_RLL_P.Enabled = enable;//(0, 0, 1, 0, "Q_A_RAT_RLL_P", MainUI.comPort.MAV.param);
            Q_A_RAT_RLL_I.Enabled = enable;//(0, 0, 100, 0, "Q_A_RAT_RLL_I", MainUI.comPort.MAV.param);
            Q_A_RAT_RLL_D.Enabled = enable;//(0, 0, 100, 0, "Q_A_RAT_RLL_D", MainUI.comPort.MAV.param);
            Q_A_RAT_RLL_IMAX.Enabled = enable;//(0, 0, 100, 0, "Q_A_RAT_RLL_IMAX", MainUI.comPort.MAV.param);
            Q_A_RAT_RLL_FF.Enabled = enable;//(0, 0, 100, 0, "Q_A_RAT_RLL_FF", MainUI.comPort.MAV.param);
            Q_A_RAT_RLL_FILT.Enabled = enable;//(0, 0, 1, 0, "Q_A_RAT_RLL_FILT", MainUI.comPort.MAV.param);
            #endregion

            #region PITCH速率
            Q_A_RAT_PIT_P.Enabled = enable;//(0, 0, 1, 0, "Q_A_RAT_PIT_P", MainUI.comPort.MAV.param);
            Q_A_RAT_PIT_I.Enabled = enable;//(0, 0, 100, 0, "Q_A_RAT_PIT_I", MainUI.comPort.MAV.param);
            Q_A_RAT_PIT_D.Enabled = enable;//(0, 0, 1, 0, "Q_A_RAT_PIT_D", MainUI.comPort.MAV.param);
            Q_A_RAT_PIT_IMAX.Enabled = enable;//(0, 0, 1, 0, "Q_A_RAT_PIT_IMAX", MainUI.comPort.MAV.param);
            Q_A_RAT_PIT_FF.Enabled = enable;//(0, 0, 1, 0, "Q_A_RAT_PIT_FF", MainUI.comPort.MAV.param);
            Q_A_RAT_PIT_FILT.Enabled = enable;//(0, 0, 100, 0, "Q_A_RAT_PIT_FILT", MainUI.comPort.MAV.param);

            #endregion

            #region YAW 速率
            Q_A_RAT_YAW_P.Enabled = enable;//(0, 0, 1, 0, "Q_A_RAT_YAW_P", MainUI.comPort.MAV.param);
            Q_A_RAT_YAW_I.Enabled = enable;//(0, 0, 1, 0, "Q_A_RAT_YAW_I", MainUI.comPort.MAV.param);
            Q_A_RAT_YAW_D.Enabled = enable;//(0, 0, 1, 0, "Q_A_RAT_YAW_D", MainUI.comPort.MAV.param);
            Q_A_RAT_YAW_IMAX.Enabled = enable;//(0, 0, 100, 0, "Q_A_RAT_YAW_IMAX", MainUI.comPort.MAV.param);
            Q_A_RAT_YAW_FF.Enabled = enable;//(0, 0, 1, 0, "Q_A_RAT_YAW_FF", MainUI.comPort.MAV.param);
            Q_A_RAT_YAW_FILT.Enabled = enable;//(0, 0, 1, 0, "Q_A_RAT_YAW_FILT", MainUI.comPort.MAV.param);
            #endregion
            
            #region 旋翼/固定翼模式转换

            Q_TRANSITION_MS.Enabled = enable;//(0, 0, 100, 0, "Q_TRANSITION_MS", MainUI.comPort.MAV.param);
            Q_ASSIST_SPEED.Enabled = enable;//(0, 0, 1, 0, "Q_ASSIST_SPEED", MainUI.comPort.MAV.param);
            Q_ASSIST_ANGLE.Enabled = enable;//(0, 0, 1, 0, "Q_ASSIST_ANGLE", MainUI.comPort.MAV.param);
            Q_M_HOVER_LEARN.Enabled = enable;//(1, 0, "Q_M_HOVER_LEARN", MainUI.comPort.MAV.param);
            #endregion

            #region 航点导航
            Q_GUIDED_MODE.Enabled = enable;//(1, 0, "Q_GUIDED_MODE", MainUI.comPort.MAV.param);
            Q_RTL_MODE.Enabled = enable;//(1, 0, "Q_RTL_MODE", MainUI.comPort.MAV.param);
            Q_RTL_ALT.Enabled = enable;//(0, 0, 1, 0, "Q_RTL_ALT", MainUI.comPort.MAV.param);
            Q_WP_SPEED.Enabled = enable;//(0, 0, 1, 0, "Q_WP_SPEED", MainUI.comPort.MAV.param);
            Q_WP_RADIUS.Enabled = enable;//(0, 0, 100, 0, "Q_WP_RADIUS", MainUI.comPort.MAV.param);
            Q_WP_SPEED_UP.Enabled = enable;//(0, 0, 1, 0, "Q_WP_SPEED_UP", MainUI.comPort.MAV.param);
            Q_WP_SPEED_DN.Enabled = enable;//(0, 0, 1, 0, "Q_WP_SPEED_DN", MainUI.comPort.MAV.param);
            Q_LAND_SPEED.Enabled = enable;//(0, 0, 1, 0, "Q_LAND_SPEED", MainUI.comPort.MAV.param);
            Q_LAND_FINAL_ALT.Enabled = enable;//(0, 0, 1, 0, "Q_LAND_FINAL_ALT", MainUI.comPort.MAV.param);
            #endregion

            #region 倾转垂起功能
           
            Q_TILT_MASK_1.Enabled = enable;
            Q_TILT_MASK_2.Enabled = enable;
            Q_TILT_MASK_3.Enabled = enable;
            Q_TILT_MASK_4.Enabled = enable;
            Q_TILT_MASK_5.Enabled = enable;
            Q_TILT_MASK_6.Enabled = enable;
            Q_TILT_MASK_7.Enabled = enable;
            Q_TILT_MASK_8.Enabled = enable;

            Q_TILT_TYPE.Enabled = enable;//(ParameterMetaDataRepository.GetParameterOptionsInt("Q_TILT_TYPE", MainUI.comPort.MAV.cs.firmware.ToString()).ToList(), "Q_TILT_TYPE", MainUI.comPort.MAV.param);
            Q_TILT_RATE_UP.Enabled = enable;//(0, 0, 1, 0, "Q_TILT_RATE_UP", MainUI.comPort.MAV.param);
            Q_TILT_RATE_DN.Enabled = enable;//(0, 0, 1, 0, "Q_TILT_RATE_DN", MainUI.comPort.MAV.param);
            Q_TILT_MAX.Enabled = enable;//(0, 0, 1, 0, "Q_TILT_MAX", MainUI.comPort.MAV.param);
            Q_TILT_YAW_ANGLE.Enabled = enable;//(0, 0, 1, 0, "Q_TILT_YAW_ANGLE", MainUI.comPort.MAV.param);
            #endregion

            #region 尾座式垂起功能
            Q_TAILSIT_ANGLE.Enabled = enable;//(0, 0, 1, 0, "Q_TAILSIT_ANGLE", MainUI.comPort.MAV.param);
            Q_TAILSIT_INPUT.Enabled = enable;//(ParameterMetaDataRepository.GetParameterOptionsInt("Q_TAILSIT_INPUT", MainUI.comPort.MAV.cs.firmware.ToString()).ToList(), "Q_TAILSIT_INPUT", MainUI.comPort.MAV.param);
            Q_TAILSIT_MASK.Enabled = enable;//(0, 0, 1, 0, "Q_TAILSIT_MASK", MainUI.comPort.MAV.param);

            Q_TAILSIT_MASKCH.Enabled = enable;//(ParameterMetaDataRepository.GetParameterOptionsInt("Q_TAILSIT_MASKCH", MainUI.comPort.MAV.cs.firmware.ToString()).ToList(), "Q_TAILSIT_MASKCH", MainUI.comPort.MAV.param);
            #endregion

        }



        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.S))
            {
                BUT_writePIDS_Click(null, null);
                return true;
            }

            return false;
        }

        private static string AddNewLinesForTooltip(string text)
        {
            if (text.Length < maximumSingleLineTooltipLength)
                return text;
            var lineLength = (int)Math.Sqrt(text.Length) * 2;
            var sb = new StringBuilder();
            var currentLinePosition = 0;
            for (var textIndex = 0; textIndex < text.Length; textIndex++)
            {
                // If we have reached the target line length and the next      
                // character is whitespace then begin a new line.   
                if (currentLinePosition >= lineLength &&
                    char.IsWhiteSpace(text[textIndex]))
                {
                    sb.Append(Environment.NewLine);
                    currentLinePosition = 0;
                }
                // If we have just started a new line, skip all the whitespace.    
                if (currentLinePosition == 0)
                    while (textIndex < text.Length && char.IsWhiteSpace(text[textIndex]))
                        textIndex++;
                // Append the next character.     
                if (textIndex < text.Length) sb.Append(text[textIndex]);
                currentLinePosition++;
            }
            return sb.ToString();
        }

        private void ComboBox_Validated(object sender, EventArgs e)
        {
            EEPROM_View_float_TextChanged(sender, e);
        }

        private void Configuration_Validating(object sender, CancelEventArgs e)
        {
            EEPROM_View_float_TextChanged(sender, e);
        }

        void EEPROM_View_float_TextChanged(object sender, EventArgs e)
        {
            float value = 0;
            var name = ((Control)sender).Name;

            // do domainupdown state check
            try
            {
                if (sender.GetType() == typeof(MavlinkNumericUpDown))
                {
                    value = ((MAVLinkParamChanged)e).value;
                    changes[name] = value;
                }
                else if (sender.GetType() == typeof(MavlinkComboBox))
                {
                    value = ((MAVLinkParamChanged)e).value;
                    changes[name] = value;
                }
                ((Control)sender).BackColor = Color.Green;
            }
            catch (Exception)
            {
                ((Control)sender).BackColor = Color.Red;
            }
        }

        /// <summary>
        ///     Handles the Click event of the BUT_rerequestparams control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        protected void BUT_rerequestparams_Click(object sender, EventArgs e)
        {
            if (!MainUI.comPort.BaseStream.IsOpen)
                return;

            ((Control)sender).Enabled = false;

            try
            {
                MainUI.comPort.getParamList();
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Error: getting param list " + ex, Strings.ERROR);
            }


            ((Control)sender).Enabled = true;

            Activate();
        }

        private void BUT_refreshpart_Click(object sender, EventArgs e)
        {
            if (!MainUI.comPort.BaseStream.IsOpen)
                return;

            ((Control)sender).Enabled = false;


            updateparam(this);

            ((Control)sender).Enabled = true;


            Activate();
        }

        private void updateparam(Control parentctl)
        {
            foreach (Control ctl in parentctl.Controls)
            {
                if (typeof(NumericUpDown) == ctl.GetType() || typeof(ComboBox) == ctl.GetType())
                {
                    try
                    {
                        MainUI.comPort.GetParam(ctl.Name);
                    }
                    catch
                    {
                    }
                }

                if (ctl.Controls.Count > 0)
                {
                    updateparam(ctl);
                }
            }
        }

        private void numeric_ValueUpdated(object sender, EventArgs e)
        {
            EEPROM_View_float_TextChanged(sender, e);
        }

        private void Q_ENABLE_CheckStateChanged(object sender, EventArgs e)
        {
            SetQ_ParametersEnable(Q_ENABLE.Checked);
        }
            
        private void Q_TILT_MASK_1_CheckStateChanged(object sender, EventArgs e)
        {
            if (startup) return;
            int value = getQ_TILT_MASK_Params();
            changes["Q_TILT_MASK"] = value;
        }

        private void BUT_writePIDS_Click(object sender, EventArgs e)
        {
            var temp = (Hashtable)changes.Clone();

            foreach (string value in temp.Keys)
            {
                try
                {
                    //if ((float)changes[value] > (float)MainUI.comPort.MAV.param[value] * 2.0f)
                    //    if (
                    //        CustomMessageBox.Show(value + " has more than doubled the last input. Are you sure?",
                    //            "Large Value", MessageBoxButtons.YesNo) == (int)DialogResult.No)
                    //    {
                    //        try
                    //        {
                    //            // set control as well
                    //            var textControls = Controls.Find(value, true);
                    //            if (textControls.Length > 0)
                    //            {
                    //                // restore old value
                    //                textControls[0].Text = MainUI.comPort.MAV.param[value].Value.ToString();
                    //                textControls[0].BackColor = Color.FromArgb(0x43, 0x44, 0x45);
                    //            }
                    //        }
                    //        catch
                    //        {
                    //        }
                    //        return;
                    //    }


                    if (value == "Q_TILT_MASK") MainUI.comPort.setParam(value, (int)changes[value]);
                    else MainUI.comPort.setParam(value, (float)changes[value]);

                    changes.Remove(value);

                    try
                    {
                        // set control as well
                        var textControls = Controls.Find(value, true);
                        if (textControls.Length > 0)
                        {
                            textControls[0].BackColor = Color.FromArgb(0x43, 0x44, 0x45);
                        }
                    }
                    catch
                    {
                    }
                }
                catch
                {
                    CustomMessageBox.Show(string.Format(Strings.ErrorSetValueFailed, value), Strings.ERROR);
                }
            }
        }
    }
}
