using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


//
public class Tracks_2Behaviour : Jundroo.SimplePlanes.ModTools.Parts.PartModifierBehaviour
{
    #region Description
    GameObject attach_points_papa;
    const float from_kmh_to_ms = 0.277f;
    const float from_ms_to_kmh = 3.6f;


    public GameObject Tracks_part;
    public GameObject All;
    public GameObject Torsions_papa;
    public GameObject Torsions_papa2;
    public GameObject[] Torsions;
    Rigidbody Main_Rigidbody;

    public GameObject Main_wheel_mount;
    public GameObject Gear_wheel_mount;
    public GameObject Free_wheel_mount;
    public GameObject Support_wheel_mount;

    public GameObject Free_whell_collider;
    public GameObject Gear_whell_collider;

    public GameObject Back_obj;
    public GameObject Forward_obj;

    public GameObject Up_obj;
    public GameObject Down_obj;

    public GameObject Tracks_box_collider;

    public GameObject free_wheel_tracks_sign_point;

 
   

    GameObject Gear_wheel_shape;
    GameObject Free_wheel_shape;
    GameObject[] Support_wheel_shapes;
    GameObject[] Support_wheel_derjatels;

    GameObject[] Main_wheel_mounts;
    GameObject[] Main_wheel_shapes;
    GameObject[] Main_wheel_forces;
    GameObject[] Main_wheel_points;

    GameObject[] Support_wheel_mounts;
    GameObject[] Support_wheel_points;

    GameObject Free_wheel_points;
    GameObject Gear_wheel_point;

    GameObject First_main_wheel_front_point;
    GameObject First_main_wheel_back_point;

   

    float[] hit_distance;
    Vector3[] normal_ground_direction;
    Vector3[] true_forward_direction;
    Vector3[] true_right_direction;
    float[] hit_distance_clamp;
    RaycastHit[] hits;

    bool[] hit_bools;

    float[] L;
    float[] suspension_wheel_force;

    float[] damping;
    float[] friction_x;
    float[] friction_force_x;
    float[] friction_z;
    float[] friction_force_z;

    float[] expect_friction_x;
    float[] expect_friction_force_x;
    float[] expect_friction_z;
    float[] expect_friction_force_z;

    Vector3[] world_linear_speed;
    Vector3[] locale_linear_speed;

    float[] x_speed;
    float[] z_speed;
    float z_speed_all;
    float[,] suspension_speed;
    int ca1 = 0;
    int ca2 = 0;
    float[] speed;
    float[] angle_wheel_speed;
    float[] wheel_rot_increase;
    float wheel_rot_gear_increase;
    float wheel_rot_free_increase;
    float wheel_rot_supp_increase;
    float[] forward_force;

    int[] hits_of_wheels;

    float output_hydro_pitch;
    float output_hydro_roll;
    float output_hydro_all;
    

    Jundroo.SimplePlanes.ModTools.Parts.IInputController[] inputs = new Jundroo.SimplePlanes.ModTools.Parts.IInputController[6];
    float input_throttle;
    float input_pitch;
    float input_brake;
    float input_hydro_pitch;
    float input_hydro_roll;
    float input_hydro_all;
    int side;

    //const float fift_sixt = 0.833f;//из 50 кадров в секунду в 60, для такой же силы, как если бы сила прикладывалась 60 раз в секунду, а не 50.
    const float fift_sixt = 1f;
    const float kgf_to_N = 10f;// из кгс в ньютоны!!!
    const float side_fric = 5f;//делитель поперечного трения
    const float forw_fric = 45f;//делитель продольного трения
    const float susp_damp = 25f;//делитель демпфирования



    float[] mass_designer = new float[100000];
    int mdi = 0;
    bool answer;


    //////////////////MODIFERS
    // get_modifers_at_start()
    float m_main_wheel_radius;
    float m_gear_wheel_radius;
    float m_free_wheel_radius;
    float m_support_wheel_radius;
    float m_Gear_to_first_main_length;
    float m_Free_to_first_support_length;
    float m_Main_wheel_spacing;
    float m_Support_wheel_spacing;
    float m_Main_wheel_mount_offset;
    float m_Support_wheel_mount_offset;
    float m_Gear_wheel_mount_offset;
    int m_wheel_count;
    int m_support_wheel_count;
    float m_Tracks_length;
    float m_tracks_thickness;
   
    string m_Side;
    float m_power;
    float m_suspension_spring;
    float m_suspension_distance;
    float m_suspension_damper;
    float m_wheel_friction_side;
    float m_wheel_friction_forward;

    float m_divider;
    float m_lever_length;
    float m_lever_offset;
    int m_invisible_support_wheels;
    float m_brake_force;
    float m_tracks_width;

    float m_main_wheel_width;
    float m_free_wheel_width;
    float m_gear_wheel_width;
    float m_support_wheel_width;

    float m_horizontal_main_wheel_offset;
    float m_horizontal_free_wheel_offset;
    float m_horizontal_gear_wheel_offset;
    float m_horizontal_support_wheel_offset;

    float m_x_offset_1_3_5;
    float m_x_offset_2_4_6;

    float m_hydraulic_speed;
    bool m_hydraulic_system_bool;
    float m_hydraulic_pitch_force;

    float m_maximal_speed_forward;
    float m_antispeed_multiplier_forward;
    float m_maximal_speed_back;
    float m_antispeed_multiplier_back;
    //float m_negative_maximal_speed;


    //////////////////MODIFERS

    public AnimationCurve torsions_move;

    //////////////////////////////////////////////////////////TRACKS

    //различные объекты
    GameObject First_main_offset_down_point;
    GameObject First_main_offset_down_point_angle_0;

    GameObject First_main_angled_rotation_down_point;
    GameObject First_main_angled_offset_down_point;

    GameObject Free_down_rotation_point;
    GameObject Free_down_offset_point;

    

    GameObject Free_angled_down_rotation_point;
    GameObject Free_angled_down_offset_point;
    Vector3 from_main_to_free_tracks_direction_vec;
    float angle_betwen_UP_and_main_down_to_free_down_TWO = 0f;

    Vector3 from_main_to_free_tracks_direction_vec3;
    float angle_betwen_UP_and_main_down_to_free_down_THREE = 0f;

    GameObject Free_up_rotation_point;
    GameObject Free_up_offset_point;

    GameObject Gear_down_rotation_point;
    GameObject Gear_down_offset_point;

    GameObject Gear_up_rotation_point;
    GameObject Gear_up_offset_point;

    GameObject First_support_up_point;
    GameObject Last_support_up_point;



    Transform[] Tracks_in_children = new Transform[500];
    //различные направления и значения
    const float track_high = 43.7f / 1000f;
    Vector3 Up_direction;
    float angle_betwen_UP_and_main_to_free = 0f;
    Vector3 from_down_main_to_down_free_direct;
    float angle_betwen_UP_and_main_down_to_free_down = 0f;

    float angle_betwen_up_and_free_to_sup = 0f;
    float angle_betwen_up_and_free_up_to_sup_up = 0f;
    Vector3 from_free_up_to_sup_up_direct;

    float angle_betwen_up_and_sup_to_gear = 0f;
    float angle_betwen_up_and_sup_up_to_gear_up = 0f;
    Vector3 from_sup_up_to_gear_up_direct;


    float angle_betwen_UP_and_gear_to_main = 0f;
    Vector3 from_down_gear_to_down_main_direct;
    float angle_betwen_UP_and_gear_down_to_main_down = 0f;

    float main_angled_front_direction_length = 0f;
    //траки основных катков
    const float track_part_length = 0.17f;
    const float minimal_track_length = track_part_length;
    int main_tracks_count = 0;
    GameObject[] track_direction;
    GameObject[,] main_tracks_mass;
    
    public AnimationCurve main_tracks_position_curve;
    float tracks_position_speed = 0f;


    //трак от центра переднего катка к касательной
    GameObject main_direction_front_rotation_point;
    GameObject main_direction_front_direction;


    public AnimationCurve angled_front_tracks_position_curve;
    
    //касательная
    GameObject Last_main_offset_down_point;
    GameObject Last_main_offset_down_point_angle_0;
    GameObject Last_main_rotation_point;
    GameObject from_main_to_free_direction;
    GameObject from_main_to_free_direction_rotation_point;
    public AnimationCurve from_main_to_free_tracks_position_curve;


    //ленивец вращение
    GameObject free_direction_rotation_point;
  
    
    public AnimationCurve free_tracks_position_curve;

    //к поддерживающему катку
    GameObject from_free_to_sup_direction;
    GameObject from_free_to_sup_direction_rotation_point;
    public AnimationCurve from_free_to_sup_tracks_position_curve;

    //траки поддерживающей ленты
    GameObject[] sup_direction;
    public AnimationCurve sup_tracks_position_curve;
    GameObject[,] sup_tracks_mass;
    int sup_tracks_count = 0;

    //от поддерживающего к зубчатому
    GameObject from_sup_to_gear_direction;
    
    public AnimationCurve from_sup_to_gear_tracks_position_curve;


    //касательная от зубчатого
    GameObject from_gear_to_main_direction;
    GameObject from_gear_to_main_direction_rotation_point;

    public AnimationCurve from_gear_to_main_tracks_position_curve;

    //от касательной к основному катку

    GameObject from_gear_to_main_napr_direction_offset_point;
    GameObject from_gear_to_main_napr_direction_rotation_point;
    GameObject from_gear_to_main_napr_direction_rotation_point_offset_proverka;
    GameObject from_gear_to_main_napr_direction;
    public AnimationCurve from_gear_to_main_napr_tracks_position_curve;

    //ЗУБЧАТОЕ

    GameObject gear_direction_rotation_point;
    public AnimationCurve gear_tracks_position_curve;


    //Копии самих траков
    public GameObject Main_tracks_part_papa;
    public GameObject Main_tracks_part_first_track;

    public GameObject Sup_tracks_part_papa;
    public GameObject Sup_tracks_part_first_track;

    public GameObject Free_tracks_part_papa;
    public GameObject Free_tracks_part_first_track;

    public GameObject from_main_to_free_tracks_part_papa;
    public GameObject from_main_to_free_tracks_part_first_track;

    public GameObject front_main_angled_tracks_part_papa;
    public GameObject front_main_angled_tracks_part_first_track;

    public GameObject from_free_to_sup_tracks_part_papa;
    public GameObject from_free_to_sup_tracks_part_first_track;

    public GameObject Gear_tracks_part_papa;
    public GameObject Gear_tracks_part_first_track;

    public GameObject from_gear_to_main_tracks_part_papa;
    public GameObject from_gear_to_main_tracks_part_first_track;

    public GameObject Gear_to_main_tracks_part_papa;
    public GameObject Gear_to_main_tracks_part_first_track;

    public GameObject from_sup_to_gear_tracks_part_papa;
    public GameObject from_sup_to_gear_tracks_part_first_track;



    #endregion

    void Start()
    {
        Up_dir();
        inputs = Tracks_part.GetComponentsInChildren<Jundroo.SimplePlanes.ModTools.Parts.IInputController>();
        get_modifers_at_start();

        //Здесь я работаю только с колесами до копирования, поэтому они вызываются до создания массива.
        Side();
        all_wheels_size();
        all_wheels_pos_xz();
        // tracks_thickness();//траки надо переделать
        //Создание массивов объектов
        creating_arrays_at_start();
        Lever();
        create_tracks_part_copyes();
       
        Wheel_copyes();
        chess_wheels();
        //if(!ServiceProvider.Instance.GameState.IsInDesigner)
        //{
        //    chess_wheels_start_level();
        //}

        get_torsions();
        ////////////////////////////////////////////TRACKS
        Tracks_start();


        Torsions_out();
        Torsions_out2();
        Shapes_point_forces_of_wheels();

        if (ServiceProvider.Instance.GameState.IsInDesigner)
        {
            get_attach_point_papa();
            set_attach_points();

        }
            

        if (!ServiceProvider.Instance.GameState.IsInDesigner)
        {
            destr_all_tracks_col();
            Delete_lishnee();
            Find_tracks();
        }
       
    }

    void Update()
    {//
        Up_dir();
        if (ServiceProvider.Instance.GameState.IsInDesigner)
        {
            Designer_timer();

            if (answer == true)
            {


                foreach (var mou in Main_wheel_mounts)
                {
                    DestroyImmediate(mou);
                }

                foreach (var mou in Support_wheel_mounts)
                {
                    DestroyImmediate(mou);
                }
                Destroy_torsions();
                Destroy_torsions2();
                
                get_modifers_at_start();

                //Здесь я работаю только с колесами до копирования, поэтому они вызываются до создания массива.
                Side();
                all_wheels_size();
                all_wheels_pos_xz();
                // tracks_thickness();//траки надо переделать
                //Создание массивов объектов
                creating_arrays_at_start();
                Lever();
               
                Wheel_copyes();

               

                get_torsions();
                ////////////////////////////////////////////TRACKS
                //Tracks_start();222222


                Torsions_out();
                Torsions_out2();
                Shapes_point_forces_of_wheels();

                int i = 0;
                do
                {
                        L[i] = 0f;
                    hit_distance_clamp[i] = m_suspension_distance;
                    i++;
                } while (i < m_wheel_count);


                Update_main_Wheel_position();
                Mount_move();//
                ////////////////////////////////////////////TRACKS


            }
            //Tracks_update();111111
           
            tracks_col_scale();
            chess_wheels();
        }



        if (!ServiceProvider.Instance.GameState.IsInDesigner && !ServiceProvider.Instance.GameState.IsPaused)
        {
            get_main_rigidbody();

            InputControl();
            output_hydro();
            is_wheel_grounded();

            //wheel_speed_y();


            Update_main_Wheel_position();
            get_wheel_mount_speed();
            main_wheel_friction();
            expect_Friction();
            wheel_rotation();

            ////////////////////////////////////////////TRACKS
            
            Tracks_update();
            show_something();
        }
    }

    void FixedUpdate()
    {
        if (!ServiceProvider.Instance.GameState.IsInDesigner && !ServiceProvider.Instance.GameState.IsPaused)
        {

            wheel_speed_y(); 
            Wheel_forces();

            Wheel_force_forward();

            Mount_move();
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    void get_main_rigidbody()
    {
        Main_Rigidbody = Tracks_part.transform.parent.gameObject.transform.parent.gameObject.GetComponent<Rigidbody>();
    }

    void get_attach_point_papa()
    {
        attach_points_papa = Tracks_part.transform.Find("AttachPoints").gameObject;
    }

    void set_attach_points()
    {

        attach_points_papa.transform.GetChild(0).localPosition = new Vector3(-0.723f, 0f, 0f);
        attach_points_papa.transform.GetChild(1).localPosition = new Vector3(-0.723f, 0f, 1f);
        attach_points_papa.transform.GetChild(2).localPosition = new Vector3(-0.723f, 0f, 2f);
        attach_points_papa.transform.GetChild(3).localPosition = new Vector3(-0.723f, 0f, -1f);
        attach_points_papa.transform.GetChild(4).localPosition = new Vector3(-0.723f, 0f, -2f);

        attach_points_papa.transform.GetChild(5).localPosition = new Vector3(0.723f, 0f, 0f);
        attach_points_papa.transform.GetChild(6).localPosition = new Vector3(0.723f, 0f, 1f);
        attach_points_papa.transform.GetChild(7).localPosition = new Vector3(0.723f, 0f, 2f);
        attach_points_papa.transform.GetChild(8).localPosition = new Vector3(0.723f, 0f, -1f);
        attach_points_papa.transform.GetChild(9).localPosition = new Vector3(0.723f, 0f, -2f);
        



    }

  

    void get_modifers_at_start()
    {
        var modifer = (Tracks_2)PartModifier;
       
        m_lever_length = Mathf.Clamp(modifer.suspension_lever_length, 0.4f, 1000f);
        m_wheel_count = Mathf.Clamp(modifer.wheel_count, 2, 100);
        m_main_wheel_radius = Mathf.Clamp(modifer.main_wheel_radius,0.05f,100f);
        m_gear_wheel_radius = Mathf.Clamp(modifer.gear_wheel_radius, 0.05f, 100f);
        m_free_wheel_radius = Mathf.Clamp(modifer.free_wheel_radius, 0.05f, 100f);
        m_support_wheel_radius = Mathf.Clamp(modifer.support_wheel_radius, 0.05f, 100f);
        m_Gear_to_first_main_length = Mathf.Clamp(modifer.Gear_to_first_main_length, m_lever_length, 100f);
        
        m_Free_to_first_support_length = modifer.Free_to_first_support_length;
      
        m_Main_wheel_spacing = Mathf.Clamp(modifer.Main_wheel_spacing, 0.05f, 100f);
        float limit_track_length__value = m_Gear_to_first_main_length + m_Main_wheel_spacing * (m_wheel_count - 1) + m_lever_length/2f;
        m_Tracks_length = Mathf.Clamp(modifer.Tracks_length, limit_track_length__value, 100f);


        //float limit_sup_wheel_spacing = m_Gear_to_first_main_length + m_Main_wheel_spacing * (m_wheel_count - 1) + m_lever_length / 2f;
        m_Support_wheel_spacing = modifer.Support_wheel_spacing;
        m_Main_wheel_mount_offset = modifer.Main_wheel_mount_offset;
        m_Support_wheel_mount_offset = modifer.Support_wheel_mount_offset;
        m_Gear_wheel_mount_offset = modifer.Gear_wheel_mount_offset;
        m_support_wheel_count = Mathf.Clamp(modifer.support_wheel_count,2,100);

        //m_tracks_thickness = modifer.tracks_thickness;
        m_tracks_thickness = 0.0737f;
        m_Side = modifer.Side;
        m_power = modifer.power;
        m_suspension_spring = modifer.suspension_spring;

        m_lever_offset = Mathf.Clamp(modifer.suspension_lever_offset, 0.1f, m_lever_length);
        float val1 = (m_lever_length + m_main_wheel_radius + m_tracks_thickness);
        m_suspension_distance = Mathf.Clamp(val1 - m_lever_offset,0.1f,100f);
        m_suspension_damper = modifer.suspension_damper;
        m_wheel_friction_side = modifer.wheel_friction_side;
        m_wheel_friction_forward = modifer.wheel_friction_forward;

        if(modifer.is_support_wheels_visible == "Yes")
        {
            m_invisible_support_wheels = 1;
        }

        if (modifer.is_support_wheels_visible == "No")
        {
            m_invisible_support_wheels = 0;
        }
        m_brake_force = modifer.brake_force;

        m_tracks_width = modifer.Tracks_width;

        m_main_wheel_width = modifer.main_wheel_width;
        m_free_wheel_width = modifer.free_wheel_width;
        m_gear_wheel_width = modifer.gear_wheel_width;
        m_support_wheel_width = modifer.support_wheel_width;

        
        m_horizontal_free_wheel_offset = modifer.Free_wheel_horizontal_offset;
        m_horizontal_gear_wheel_offset = modifer.Gear_wheel_horizontal_offset;
        m_horizontal_support_wheel_offset = modifer.Support_wheel_horizontal_offset;
        m_x_offset_1_3_5 = modifer.x_offset_of_1_3_5;
        m_x_offset_2_4_6 = modifer.x_offset_of_2_4_6;
        m_hydraulic_speed = modifer.hydraulic_speed;
        m_hydraulic_pitch_force = modifer.hydraulic_pitch_force;

        m_maximal_speed_forward = modifer.maximal_speed_forward;
        m_antispeed_multiplier_forward = modifer.antispeed_multiplier_forward;
        m_maximal_speed_back= modifer.maximal_speed_back;
        m_antispeed_multiplier_back = modifer.antispeed_multiplier_back;

        if (modifer.is_hydraulic_suspension_exist == "Yes")
        {
            m_hydraulic_system_bool = true;
        }

        if (modifer.is_hydraulic_suspension_exist == "No")
        {
            m_hydraulic_system_bool = false;
        }
    }


    void creating_arrays_at_start()
    {

        Main_wheel_mounts = new GameObject[m_wheel_count];
        Main_wheel_shapes = new GameObject[m_wheel_count];
        Main_wheel_points = new GameObject[m_wheel_count];
        Main_wheel_forces = new GameObject[m_wheel_count];
        Torsions = new GameObject[m_wheel_count];

        Support_wheel_mounts = new GameObject[m_support_wheel_count];
        Support_wheel_points = new GameObject[m_support_wheel_count];
        Support_wheel_shapes = new GameObject[m_support_wheel_count];
        Support_wheel_derjatels = new GameObject[m_support_wheel_count];
        hits = new RaycastHit[m_wheel_count];
        hit_distance = new float[m_wheel_count];
        hit_distance_clamp = new float[m_wheel_count];
        normal_ground_direction = new Vector3[m_wheel_count];
        true_forward_direction = new Vector3[m_wheel_count];
        true_right_direction = new Vector3[m_wheel_count];

        hit_bools = new bool[m_wheel_count];
        hits_of_wheels = new int[m_wheel_count];
        L = new float[m_wheel_count];
        suspension_wheel_force = new float[m_wheel_count];

        damping = new float[m_wheel_count];
        friction_x = new float[m_wheel_count];
        friction_force_x = new float[m_wheel_count];
        friction_z = new float[m_wheel_count];
        friction_force_z = new float[m_wheel_count];

        expect_friction_x = new float[m_wheel_count];
        expect_friction_force_x = new float[m_wheel_count];
        expect_friction_z = new float[m_wheel_count];
        expect_friction_force_z = new float[m_wheel_count];

        forward_force = new float[m_wheel_count];

        world_linear_speed = new Vector3[m_wheel_count];
        locale_linear_speed = new Vector3[m_wheel_count];

        x_speed = new float[m_wheel_count];
        z_speed = new float[m_wheel_count];

        suspension_speed = new float[10000, m_wheel_count];
        speed = new float[m_wheel_count];
        angle_wheel_speed = new float[m_wheel_count];
        wheel_rot_increase = new float[m_wheel_count];

      
        ////////////////////////////////////////////////////////TRACKS
        track_direction = new GameObject[m_wheel_count - 1];
        main_tracks_count = Mathf.Clamp(Mathf.FloorToInt(m_Main_wheel_spacing / track_part_length),1,100);
        main_tracks_mass = new GameObject[m_wheel_count - 1, main_tracks_count];

        sup_direction = new GameObject[m_support_wheel_count-1];
        sup_tracks_count = Mathf.Clamp(Mathf.FloorToInt(m_Support_wheel_spacing / track_part_length), 1, 100);
        sup_tracks_mass = new GameObject[m_support_wheel_count - 1, sup_tracks_count];
    }


    void Wheel_copyes()
    {
        

        Main_wheel_mount.SetActive(true);

        int i = 0;
        do {
            GameObject Copy_wheel = (GameObject)Instantiate(Main_wheel_mount, Main_wheel_mount.transform.position, Main_wheel_mount.transform.rotation);
            Copy_wheel.transform.parent = All.transform;
            Copy_wheel.transform.localPosition = new Vector3(Main_wheel_mount.transform.localPosition.x, Main_wheel_mount.transform.localPosition.y, Main_wheel_mount.transform.localPosition.z + i * m_Main_wheel_spacing);
            Copy_wheel.transform.localRotation = Main_wheel_mount.transform.localRotation;
            Copy_wheel.transform.localScale = Main_wheel_mount.transform.localScale;
            Main_wheel_mounts[i] = Copy_wheel;
            i++;
        } while (i < m_wheel_count);

        Main_wheel_mount.SetActive(false);

        Support_wheel_mount.SetActive(true);

        i = 0;
        do {
            GameObject Copy_wheel = (GameObject)Instantiate(Support_wheel_mount, Support_wheel_mount.transform.position, Support_wheel_mount.transform.rotation);
            Copy_wheel.transform.parent = All.transform;
            Copy_wheel.transform.localPosition = new Vector3(Support_wheel_mount.transform.localPosition.x, Support_wheel_mount.transform.localPosition.y, Support_wheel_mount.transform.localPosition.z - i * m_Support_wheel_spacing);
            Copy_wheel.transform.localRotation = Support_wheel_mount.transform.localRotation;
            Copy_wheel.transform.localScale = Support_wheel_mount.transform.localScale;
            Support_wheel_mounts[i] = Copy_wheel;
            i++;
        } while (i < m_support_wheel_count);

        Support_wheel_mount.SetActive(false);

    }//
    void chess_wheels()
    {
        for(int i = 0;i<m_wheel_count;i=i+2)
        {
            Main_wheel_mounts[i].transform.GetChild(1).GetChild(0).localPosition = new Vector3(m_x_offset_2_4_6, 0, 0);
            //Main_wheel_mounts[i].transform.GetChild(4).GetChild(0).localPosition = new Vector3(m_x_offset_2_4_6, 0, 0);
            //Main_wheel_mounts[i].transform.GetChild(4).GetChild(1).GetChild(0).localPosition = new Vector3(m_x_offset_2_4_6, Main_wheel_mounts[i].transform.GetChild(4).GetChild(1).GetChild(0).localPosition.y, Main_wheel_mounts[i].transform.GetChild(4).GetChild(1).GetChild(0).localPosition.z);
            //Main_wheel_mounts[i].transform.GetChild(4).GetChild(1).GetChild(1).localPosition = new Vector3(m_x_offset_2_4_6, Main_wheel_mounts[i].transform.GetChild(4).GetChild(1).GetChild(1).localPosition.y, Main_wheel_mounts[i].transform.GetChild(4).GetChild(1).GetChild(1).localPosition.z);
        }

        for (int i = 1; i < m_wheel_count; i = i + 2)
        {
            Main_wheel_mounts[i].transform.GetChild(1).GetChild(0).localPosition = new Vector3(m_x_offset_1_3_5, 0, 0);
           //Main_wheel_mounts[i].transform.GetChild(4).GetChild(0).localPosition = new Vector3(m_x_offset_1_3_5, 0, 0);
           //Main_wheel_mounts[i].transform.GetChild(4).GetChild(1).GetChild(0).localPosition = new Vector3(m_x_offset_1_3_5, Main_wheel_mounts[i].transform.GetChild(4).GetChild(1).GetChild(0).localPosition.y, Main_wheel_mounts[i].transform.GetChild(4).GetChild(1).GetChild(0).localPosition.z);
           //Main_wheel_mounts[i].transform.GetChild(4).GetChild(1).GetChild(1).localPosition = new Vector3(m_x_offset_1_3_5, Main_wheel_mounts[i].transform.GetChild(4).GetChild(1).GetChild(1).localPosition.y, Main_wheel_mounts[i].transform.GetChild(4).GetChild(1).GetChild(1).localPosition.z);
        }
    }

    void chess_wheels_start_level()
    {
        for (int i = 0; i < m_wheel_count; i = i + 2)
        {
            Main_wheel_mounts[i].transform.GetChild(4).GetChild(0).localPosition = new Vector3(m_x_offset_2_4_6, 0, 0);
            Main_wheel_mounts[i].transform.GetChild(4).GetChild(1).GetChild(0).localPosition = new Vector3(m_x_offset_2_4_6, Main_wheel_mounts[i].transform.GetChild(4).GetChild(1).GetChild(0).localPosition.y, Main_wheel_mounts[i].transform.GetChild(4).GetChild(1).GetChild(0).localPosition.z);
            Main_wheel_mounts[i].transform.GetChild(4).GetChild(1).GetChild(1).localPosition = new Vector3(m_x_offset_2_4_6, Main_wheel_mounts[i].transform.GetChild(4).GetChild(1).GetChild(1).localPosition.y, Main_wheel_mounts[i].transform.GetChild(4).GetChild(1).GetChild(1).localPosition.z);
        }

        for (int i = 1; i < m_wheel_count; i = i + 2)
        {
            Main_wheel_mounts[i].transform.GetChild(4).GetChild(0).localPosition = new Vector3(m_x_offset_1_3_5, 0, 0);
            Main_wheel_mounts[i].transform.GetChild(4).GetChild(1).GetChild(0).localPosition = new Vector3(m_x_offset_1_3_5, Main_wheel_mounts[i].transform.GetChild(4).GetChild(1).GetChild(0).localPosition.y, Main_wheel_mounts[i].transform.GetChild(4).GetChild(1).GetChild(0).localPosition.z);
            Main_wheel_mounts[i].transform.GetChild(4).GetChild(1).GetChild(1).localPosition = new Vector3(m_x_offset_1_3_5, Main_wheel_mounts[i].transform.GetChild(4).GetChild(1).GetChild(1).localPosition.y, Main_wheel_mounts[i].transform.GetChild(4).GetChild(1).GetChild(1).localPosition.z);
        }
    }


    void Lever()
    {

        Main_wheel_mount.transform.GetChild(4).GetChild(1).GetChild(1).localPosition = new Vector3(0f, 0f, m_lever_length);
        Main_wheel_mount.transform.GetChild(4).GetChild(1).GetChild(0).localScale = new Vector3(25.4f, 25.4f * (m_lever_length - 0.4f) / 0.4f, 25.4f);

        Main_wheel_mount.transform.GetChild(5).GetChild(1).GetChild(1).localPosition = new Vector3(0f, 0f, m_lever_length);
        Main_wheel_mount.transform.GetChild(5).GetChild(1).GetChild(0).localScale = new Vector3(25.4f, 25.4f * (m_lever_length - 0.4f) / 0.4f, 25.4f);

    }

    void get_torsions()
    {
        for(int i = 0; i<m_wheel_count; i++)
        {
            Torsions[i] = Main_wheel_mounts[i].transform.GetChild(4).GetChild(1).gameObject;
        }
    }


    void Torsions_out()
    {
        int i = 0;
        do {
            Main_wheel_mounts[i].transform.GetChild(4).parent = Torsions_papa.transform;
            i++;
        } while (i < m_wheel_count);
    }

    void Destroy_torsions()
    {
        int i = 0;
        do {
            DestroyImmediate(Torsions_papa.transform.GetChild(m_wheel_count - 1 - i).gameObject);
            i++;
        } while (i < m_wheel_count);
    }

    void Torsions_out2()
    {
        int i = 0;
        do {
            Main_wheel_mounts[i].transform.GetChild(4).parent = Torsions_papa2.transform;
            i++;
        } while (i < m_wheel_count);
    }

    void Destroy_torsions2()
    {
        int i = 0;
        do {
            DestroyImmediate(Torsions_papa2.transform.GetChild(m_wheel_count - 1 - i).gameObject);
            i++;
        } while (i < m_wheel_count);
    }

    void Main_wheels_rays()
    {
        //
        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 4;
        
       
        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;


        int i = 0;
        do
        {
            bool hit_ray = Physics.Raycast(Main_wheel_mounts[i].transform.position, Main_wheel_mounts[i].transform.up * -1f, out hits[i], Mathf.Infinity, layerMask);
            if (hit_ray)
            {
                hit_distance[i] = hits[i].distance;
                hit_distance_clamp[i] = Mathf.Clamp(hit_distance[i], 0f, m_suspension_distance);
            }

            normal_ground_direction[i] = hits[i].normal;
            i++;
        } while (i < m_wheel_count);





        //do
        //{
        //    bool hit_ray = Physics.Raycast(Main_wheel_mounts[i].transform.position, Main_wheel_mounts[i].transform.up * -1f, out hits[i]);
        //    if (hit_ray)
        //    {
        //        hit_distance[i] = hits[i].distance;
        //        hit_distance_clamp[i] = Mathf.Clamp(hit_distance[i], 0f, m_suspension_distance);
        //    }

        //    normal_ground_direction[i] = hits[i].normal;
        //    i++;
        //} while (i < m_wheel_count);
    }

    void Suspension()
    {

        Main_wheels_rays();

        int i = 0;
        do
        {
            if (hit_bools[i] == true)
            {
                L[i] = m_suspension_distance - hit_distance_clamp[i];
            } else
                L[i] = 0f;

            i++;
        } while (i < m_wheel_count);

    }
    //NEW Торсионы не настроены
    void Mount_move()
    {
        Mount_move_animation();
        int i = 0;
        do
        {
            float pre_value = torsions_move.Evaluate(L[i] + m_lever_offset);
            float pre_value2 = (m_main_wheel_radius + m_tracks_thickness) * (torsions_move.Evaluate(L[i] + m_lever_offset) /(m_suspension_distance + m_lever_offset));
            Main_wheel_mounts[i].transform.localPosition = new Vector3(0, -m_Main_wheel_mount_offset, Gear_wheel_mount.transform.localPosition.z + m_Gear_to_first_main_length + i * m_Main_wheel_spacing - pre_value + pre_value2);
            Torsions[i].transform.LookAt(Main_wheel_shapes[i].transform,Up_direction * -1f);
            
            i++;
        } while (i < m_wheel_count);
        //ServiceProvider.Instance.GameWorld.ShowStatusMessage(torsions_move.keys[1].value.ToString() + "\n" + torsions_move.keys[1].time.ToString() + "\n" + L[m_wheel_count-1].ToString() + "\n" + (torsions_move.Evaluate(L[m_wheel_count - 1])).ToString(), 5f);
    }

    void Mount_move_animation()
    {
        torsions_move.postWrapMode = WrapMode.Clamp;
        torsions_move.preWrapMode = WrapMode.Clamp;
        Keyframe[] torsions_move_keys = torsions_move.keys;
        torsions_move_keys[0].value = 0;
        torsions_move_keys[0].time = 0;
        torsions_move_keys[1].value = m_suspension_distance + m_lever_offset;
        torsions_move_keys[1].time = m_suspension_distance + m_lever_offset;
        torsions_move.keys = torsions_move_keys;
    }


    void file_string_stream()
    {
       // string pathf = @"Stream_file_string.txt";
        //StreamReader
    }


    void Wheel_forces()
    {//


        int value = Mathf.FloorToInt(m_wheel_count / 2f);
        float[] force_add = new float[m_wheel_count];
        float[] signes = new float[m_wheel_count];
        for (int j = 0; j < value; j++)
        {
            force_add[j] = 1f - (float)((float)j / (float)value);
            signes[j] = 1f;
        }

        for (int j = 0; j < value; j++)
        {
            force_add[j+ value] = (float)((float)(j+1) / (float)(value));
            signes[j+value] = -1f;
        }

        //
        Suspension();
        int i = 0;
        do
        {
            if(m_hydraulic_system_bool)
            {

                ////////////////////////////////////main suspension    //hydro pitch//////////////////////////////////////////////////////////////////////////////////////
                suspension_wheel_force[i] = (L[i] * m_suspension_spring * kgf_to_N - damping[i]) * Mathf.Clamp((1f - force_add[i] * output_hydro_pitch * m_hydraulic_pitch_force * signes[i]), 0f, 100f) * Mathf.Clamp(1f - output_hydro_roll,0f,100f)/ m_suspension_distance * Mathf.Clamp(output_hydro_all, 0f, 100f) * Time.fixedDeltaTime * (1 / Time.timeScale);
            }
            else
            {
                
                suspension_wheel_force[i] = (L[i] * m_suspension_spring * kgf_to_N - damping[i]) / m_suspension_distance * Time.fixedDeltaTime * (1 / Time.timeScale);
            }
            
            //подвеска
            Main_Rigidbody.AddForceAtPosition(normal_ground_direction[i] * suspension_wheel_force[i], Main_wheel_mounts[i].transform.position, ForceMode.Force);
            //боковое трение
            friction_force_x[i] = friction_x[i] * Time.fixedDeltaTime * (1 / Time.timeScale);
            Main_Rigidbody.AddForceAtPosition(true_right_direction[i] * friction_force_x[i] * -1f, Main_wheel_forces[i].transform.position, ForceMode.Force);

            //продольное трение

            friction_force_z[i] = friction_z[i] * Time.fixedDeltaTime * (1 / Time.timeScale);
            Main_Rigidbody.AddForceAtPosition(true_forward_direction[i] * friction_force_z[i] * 1f, Main_wheel_forces[i].transform.position, ForceMode.Force);
            if (z_speed[i] > m_maximal_speed_forward * from_kmh_to_ms)
            {
                Main_Rigidbody.AddForceAtPosition(true_forward_direction[i] * friction_force_z[i] * (m_antispeed_multiplier_forward-1f), Main_wheel_forces[i].transform.position, ForceMode.Force);
            }

            if (z_speed[i] < m_maximal_speed_back * from_kmh_to_ms)
            {
                Main_Rigidbody.AddForceAtPosition(true_forward_direction[i] * friction_force_z[i] * (m_antispeed_multiplier_back - 1f), Main_wheel_forces[i].transform.position, ForceMode.Force);
            }
            //////трение покоя
            if (Mathf.Abs(x_speed[i]) < 4f * from_kmh_to_ms)
            {
                //боковое трение покоя
                expect_friction_force_x[i] = expect_friction_x[i] * Time.fixedDeltaTime * (1 / Time.timeScale);
                Main_Rigidbody.AddForceAtPosition(true_right_direction[i] * expect_friction_force_x[i] * -1f, Main_wheel_forces[i].transform.position, ForceMode.Force);
            }

            //продольное трение покоя
            if (Mathf.Abs(z_speed[i]) < 4f * from_kmh_to_ms)
            {
                expect_friction_force_z[i] = expect_friction_z[i] * Time.fixedDeltaTime * (1 / Time.timeScale);
                Main_Rigidbody.AddForceAtPosition(true_forward_direction[i] * expect_friction_force_z[i] * 1f, Main_wheel_forces[i].transform.position, ForceMode.Force);
            }

            //тормоза
            float z_speed_all_limit = Mathf.Clamp(z_speed_all,-0.277f,0.277f);
            float brake_force = 100f * input_brake * z_speed_all_limit * m_brake_force * -1f * Time.fixedDeltaTime * (1 / Time.timeScale);
            Main_Rigidbody.AddForceAtPosition(Main_wheel_mounts[i].transform.forward * brake_force , Main_wheel_forces[i].transform.position, ForceMode.Force);
            

            i++;

        } while (i < m_wheel_count);
        
        //ServiceProvider.Instance.GameWorld.ShowStatusMessage("\n\n\n\n\n\n\n" 
        //    + (1f - force_add[0] * signes[0] * output_hydro_pitch).ToString() + "\n"
        //    + (1f - force_add[1] * signes[1] * output_hydro_pitch).ToString() + "\n"
        //    + (1f - force_add[2] * signes[2] * output_hydro_pitch).ToString() + "\n"
        //    + (1f - force_add[3] * signes[3] * output_hydro_pitch).ToString() + "\n"
        //    + (1f - force_add[4] * signes[4] * output_hydro_pitch).ToString() + "\n"
        //    + (1f - force_add[5] * signes[5] * output_hydro_pitch).ToString() + "\n"
        //    + (1f - force_add[6] * signes[6] * output_hydro_pitch).ToString() + "\n"
        //    + (1f - force_add[7] * signes[7] * output_hydro_pitch).ToString() + "\n"
        //    + (1f - force_add[8] * signes[8] * output_hydro_pitch).ToString() + "\n"
        //     + (1f - force_add[9] * signes[9] * output_hydro_pitch).ToString() + "\n"
        //    + value.ToString() + "\n"
        //    , 5f);




    }

    void Update_main_Wheel_position()
    {

        int i = 0;
        do
        {
            Main_wheel_shapes[i].transform.localPosition = new Vector3(0, -hit_distance_clamp[i] + m_main_wheel_radius + m_tracks_thickness * 1f, 0);
            Main_wheel_points[i].transform.localPosition = new Vector3(0, -hit_distance_clamp[i] + m_main_wheel_radius + m_tracks_thickness * 1f, 0);
            i++;

        } while (i < m_wheel_count);
    }

    void get_wheel_mount_speed()
    {

        int i = 0;
        do
        {

            world_linear_speed[i] = Main_Rigidbody.GetPointVelocity(Main_wheel_mounts[i].transform.position);
            locale_linear_speed[i] = transform.InverseTransformVector(world_linear_speed[i]);

            x_speed[i] = locale_linear_speed[i].x;
            z_speed[i] = locale_linear_speed[i].z;
            i++;

        } while (i < m_wheel_count);

        z_speed_all = locale_linear_speed[0].z;
    }

    void wheel_speed_y()
    {


        ca1++;
        if (ca1 > 9998) //сброс таймера при заполнении массива
        {
            ca1 = 0;
        }

        ca2 = 0;

        do
        {
            suspension_speed[ca1, ca2] = hit_distance_clamp[ca2];
            speed[ca2] = (suspension_speed[ca1, ca2] - suspension_speed[ca1 - 1, ca2]) / Time.fixedDeltaTime;   //V.0.81.

            //speed[ca2] = (suspension_speed[ca1, ca2] - suspension_speed[ca1 - 1, ca2]) * Time.fixedDeltaTime  * 50f * 50f;// В версии 0.81 было немного неверно. При частоте обновления 0.02с ничего не поменяется, но теперь гусеницы
            //будут одинаково работать и при 0.01с, и при 0.04 с. Общая сила за секунду не изменится. А в версии 0.82 она менялась бы...Двойное умножение на 50 не работает, хз почему.
            if (hit_bools[ca2] == true)
            {
                damping[ca2] = speed[ca2] * m_suspension_damper * kgf_to_N / susp_damp;
            } else
                damping[ca2] = 0f;

            ca2++;
        } while (ca2 < m_wheel_count);

    }

    void main_wheel_friction()
    {

        int i = 0;
        do
        {

            if (hit_bools[i] == true)
            {
                friction_x[i] = x_speed[i] * m_wheel_friction_side / side_fric;
                friction_z[i] = z_speed[i] * m_wheel_friction_forward / forw_fric;
            } else
            {
                friction_x[i] = 0;
                friction_z[i] = 0;
            }

            i++;
        } while (i < m_wheel_count);

    }

    void expect_Friction()
    {

        int i = 0;
        do
        {

            if (hit_bools[i] == true)
            {
                expect_friction_x[i] = x_speed[i] * m_wheel_friction_side * 4f / side_fric;
                expect_friction_z[i] = z_speed[i] * m_wheel_friction_forward * 4f / forw_fric;
            } else
            {
                expect_friction_x[i] = 0;
                expect_friction_z[i] = 0;
            }
            i++;
        } while (i < m_wheel_count);

    }

    void wheel_rotation()
    {

        int i = 0;
        int grounded_wheels = count_grounded_wheels();
        do
        {
            angle_wheel_speed[i] = z_speed[i] / (m_main_wheel_radius + m_tracks_thickness);
            if (grounded_wheels != 0)
            {
                wheel_rot_increase[i] += angle_wheel_speed[i] * Time.deltaTime * Mathf.Rad2Deg;
            }

            Main_wheel_shapes[i].transform.localEulerAngles = new Vector3(wheel_rot_increase[i], 0, 0);
            i++;
        } while (i < m_wheel_count);


        float gear_speed = z_speed_all / (m_gear_wheel_radius + m_tracks_thickness);
        float free_speed = z_speed_all / (m_free_wheel_radius + m_tracks_thickness);
        float support_speed = z_speed_all / (m_support_wheel_radius + m_tracks_thickness);

        if (grounded_wheels != 0)
        {
            wheel_rot_gear_increase += gear_speed * Time.deltaTime * Mathf.Rad2Deg;
            Gear_wheel_shape.transform.localEulerAngles = new Vector3(wheel_rot_gear_increase, 0, 0);

            wheel_rot_free_increase += free_speed * Time.deltaTime * Mathf.Rad2Deg;
            Free_wheel_shape.transform.localEulerAngles = new Vector3(wheel_rot_free_increase, 0, 0);

            wheel_rot_supp_increase += support_speed * Time.deltaTime * Mathf.Rad2Deg;


        
            for(int j =0; j < m_support_wheel_count;j++)
            {
                Support_wheel_shapes[j].transform.localEulerAngles = new Vector3(wheel_rot_supp_increase, 0, 0);
            };


        }
    }

    void all_wheels_size()
    {

        Main_wheel_mount.transform.GetChild(1).localScale = new Vector3(m_main_wheel_width, m_main_wheel_radius / 0.4f, m_main_wheel_radius / 0.4f);
        Gear_wheel_mount.transform.GetChild(0).localScale = new Vector3(m_gear_wheel_width, m_gear_wheel_radius / 0.3763f, m_gear_wheel_radius / 0.3763f);
        Free_wheel_mount.transform.GetChild(0).localScale = new Vector3(m_free_wheel_width, m_free_wheel_radius / 0.3f, m_free_wheel_radius / 0.3f);
        Support_wheel_mount.transform.GetChild(0).GetChild(1).GetChild(0).localScale = new Vector3(m_support_wheel_width, m_support_wheel_radius / 0.1f, m_support_wheel_radius / 0.1f);

        Support_wheel_mount.transform.GetChild(0).GetChild(1).GetChild(0).localPosition = new Vector3(0.008426f + m_horizontal_support_wheel_offset/25.4f,0,0);
        Support_wheel_mount.transform.GetChild(4).localPosition = new Vector3(-0.169f-m_horizontal_support_wheel_offset,0f,0f);
        Gear_wheel_mount.transform.GetChild(0).localPosition = new Vector3(m_horizontal_gear_wheel_offset, 0, 0);
        Free_wheel_mount.transform.GetChild(0).localPosition = new Vector3(m_horizontal_free_wheel_offset, 0, 0);


        


    }

    void all_wheels_pos_xz()
    {

        Gear_wheel_mount.transform.localPosition = new Vector3(0, m_Gear_wheel_mount_offset, -m_Tracks_length / 2f);
        Free_wheel_mount.transform.localPosition = new Vector3(0, 0, m_Tracks_length / 2f);
        Main_wheel_mount.transform.localPosition = new Vector3(0, -m_Main_wheel_mount_offset, Gear_wheel_mount.transform.localPosition.z + m_Gear_to_first_main_length);
        Support_wheel_mount.transform.localPosition = new Vector3(0, -m_Support_wheel_mount_offset, Free_wheel_mount.transform.localPosition.z - m_Free_to_first_support_length);

    }

    void susp_pos_in_designer()
    {

        Gear_wheel_mount.transform.localPosition = new Vector3(0, 0, -m_Tracks_length / 2f);
        Free_wheel_mount.transform.localPosition = new Vector3(0, 0, m_Tracks_length / 2f);
        Main_wheel_mount.transform.localPosition = new Vector3(0, -m_Main_wheel_mount_offset, Gear_wheel_mount.transform.localPosition.z + m_Gear_to_first_main_length);
        Support_wheel_mount.transform.localPosition = new Vector3(0, -m_Support_wheel_mount_offset, Free_wheel_mount.transform.localPosition.z - m_Free_to_first_support_length);


        Main_wheel_mount.transform.GetChild(1).gameObject.transform.localPosition = new Vector3(0, -m_suspension_distance + m_main_wheel_radius + m_tracks_thickness * 1f, 0);
        Main_wheel_mount.transform.GetChild(3).gameObject.transform.localPosition = new Vector3(0, -m_suspension_distance + m_main_wheel_radius + m_tracks_thickness * 1f, 0);

    }

    void is_wheel_grounded()
    {

        int i = 0;
        do
        {
            if (hit_distance[i] <= hit_distance_clamp[i])
            {
                hit_bools[i] = true;
            } else
                hit_bools[i] = false;
            i++;
        } while (i < m_wheel_count);

    }

    int count_grounded_wheels()
    {

        int i = 0;
        int sum_hits = 0;
        do
        {
            if (hit_bools[i] == true)
            {
                hits_of_wheels[i] = 1;
            } else
                hits_of_wheels[i] = 0;

            sum_hits += hits_of_wheels[i];
            i++;

        } while (i < m_wheel_count);
        return sum_hits;
    }

    void InputControl()
    {

        input_throttle = inputs[0].Value;
        input_pitch = -inputs[1].Value;
        input_brake = inputs[2].Value;
        input_hydro_pitch = inputs[3].Value;
        input_hydro_roll = inputs[4].Value;
        input_hydro_all = inputs[5].Value;
    }


    void output_hydro()
    {
        float hydro_pitch_increase = m_hydraulic_speed/10 * Time.deltaTime;
        float hydro_roll_increase = m_hydraulic_speed / 10 * Time.deltaTime;
        float hydro_all_increase = m_hydraulic_speed / 10 * Time.deltaTime;
        output_hydro_pitch = (input_hydro_pitch > output_hydro_pitch) ? output_hydro_pitch + hydro_pitch_increase : output_hydro_pitch - hydro_pitch_increase;
        output_hydro_pitch = (Mathf.Abs(input_hydro_pitch - output_hydro_pitch) < 2f * hydro_pitch_increase) ? input_hydro_pitch : output_hydro_pitch;

        output_hydro_roll = (input_hydro_roll > output_hydro_roll) ? output_hydro_roll + hydro_roll_increase : output_hydro_roll - hydro_roll_increase;
        output_hydro_roll = (Mathf.Abs(input_hydro_roll - output_hydro_roll) < 2f * hydro_roll_increase) ? input_hydro_roll : output_hydro_roll;

        output_hydro_all = (input_hydro_all > output_hydro_all) ? output_hydro_all + hydro_all_increase : output_hydro_all - hydro_all_increase;
        output_hydro_all = (Mathf.Abs(input_hydro_all - output_hydro_all) < 2f * hydro_all_increase) ? input_hydro_all : output_hydro_all;
    }


    void Wheel_force_forward()
    {
        int i = 0;
        int grounded_wheels = count_grounded_wheels();
        do
        {

            if (hit_bools[i] == true)
            {
                forward_force[i] = 15f * kgf_to_N * m_power / m_wheel_count * m_wheel_count / grounded_wheels * (input_throttle + input_pitch) * Time.fixedDeltaTime * (1 / Time.timeScale);
            } else
            {
                forward_force[i] = 0;
            }
            i++;

        } while (i < m_wheel_count);

        i = 0;

        

        do
        {
            //сила движения
            //Main_Rigidbody.AddForceAtPosition(Main_wheel_mounts[i].transform.forward * forward_force[i] * 1f, Main_wheel_forces[i].transform.position, ForceMode.Force);
            //Теперь сила движения будет направлена не просто вперед, а вперед по отношению к нормали поверхности.
            true_forward_direction[i] = Vector3.Cross(normal_ground_direction[i], Main_wheel_mounts[i].transform.right);
            true_right_direction[i] = Vector3.Cross(normal_ground_direction[i], Main_wheel_mounts[i].transform.forward);
            Main_Rigidbody.AddForceAtPosition(true_forward_direction[i] * forward_force[i] * -1f, Main_wheel_forces[i].transform.position, ForceMode.Force);

            i++;
        } while (i < m_wheel_count);
    }

    void Side()
    {


        if (m_Side == "Right")
        {
            All.transform.localScale = new Vector3(1, 1, 1);

            side = 1;
        }

        if (m_Side == "Left")
        {

            All.transform.localScale = new Vector3(-1, 1, 1);

            side = -1;
        }

    }

    void Shapes_point_forces_of_wheels()
    {

        int i = 0;
        do
        {
            Main_wheel_shapes[i] = Main_wheel_mounts[i].transform.GetChild(1).gameObject;
            Main_wheel_points[i] = Main_wheel_mounts[i].transform.GetChild(3).gameObject;
            Main_wheel_forces[i] = Main_wheel_shapes[i].transform.GetChild(1).gameObject;

            i++;
        } while (i < m_wheel_count);

        Gear_wheel_shape = Gear_wheel_mount.transform.GetChild(0).gameObject;
        Free_wheel_shape = Free_wheel_mount.transform.GetChild(0).gameObject;

        i = 0;
        do
        {
            Support_wheel_shapes[i] = Support_wheel_mounts[i].transform.GetChild(0).gameObject;
            Support_wheel_derjatels[i] = Support_wheel_mounts[i].transform.GetChild(Support_wheel_mounts[i].transform.childCount-1).gameObject;
            if (m_invisible_support_wheels==1)
            {
                Support_wheel_shapes[i].SetActive(true);
                Support_wheel_derjatels[i].SetActive(true);
            }

            if (m_invisible_support_wheels == 0)
            {
                Support_wheel_shapes[i].SetActive(false);
                Support_wheel_derjatels[i].SetActive(false);
            }
            i++;
        } while (i < m_support_wheel_count);


    }

    void tracks_col_scale()
    {
        Free_whell_collider.transform.localScale = new Vector3(30.59285f * m_free_wheel_width,19.06f * m_free_wheel_radius / 0.3f, 19.06f * m_free_wheel_radius / 0.3f);
        Gear_whell_collider.transform.localScale = new Vector3(40.5f * m_gear_wheel_width, 23.718f * m_gear_wheel_radius / 0.3763f, 23.718f * m_gear_wheel_radius / 0.3763f);
        Tracks_box_collider.transform.localScale = new Vector3(1f, 1f, m_Tracks_length);
    }

    void destr_all_tracks_col()
    {
        DestroyImmediate(Tracks_box_collider);
    }



    /////////////////////////////////////////////////////////////////////////////ТРАКИ НАЧАЛО ОПИСАНИЯ

    void get_tracks_directions()
    {

        //дирекшины основных катков
        for (int i = 0; i < (m_wheel_count - 1); i++)
        {
            track_direction[i] = Main_wheel_mounts[i].transform.GetChild(6).gameObject;
        }


        //основные катки
        for (int i = 0; i < (m_wheel_count - 1); i++)
        {
            for (int j = 0; j < main_tracks_count; j++)
            {
                main_tracks_mass[i, j] = track_direction[i].transform.GetChild(j).gameObject;
            }
        }



        //основные катки передний  дирекшен
        main_direction_front_rotation_point = Main_wheel_mounts[m_wheel_count - 1].transform.GetChild(7).gameObject;
        main_direction_front_direction = Main_wheel_mounts[m_wheel_count - 1].transform.GetChild(7).GetChild(0).gameObject;


        //передний касательный диркшен     
        from_main_to_free_direction_rotation_point = Main_wheel_mounts[m_wheel_count - 1].transform.GetChild(8).gameObject;
        from_main_to_free_direction = Main_wheel_mounts[m_wheel_count - 1].transform.GetChild(8).GetChild(0).gameObject;

        //ленивец вращательный дирекшен
        free_direction_rotation_point = Free_wheel_mount.transform.GetChild(2).gameObject;
       


        //к первому поддерживающему дирекшен
        from_free_to_sup_direction_rotation_point = Free_wheel_mount.transform.GetChild(3).gameObject;
        from_free_to_sup_direction = from_free_to_sup_direction_rotation_point.transform.GetChild(0).gameObject;



        //дирекшены поддерживающих катков
        for (int i = 0; i < m_support_wheel_count - 1; i++)
        {
            sup_direction[i] = Support_wheel_mounts[i].transform.GetChild(2).gameObject;
        }

        for (int i = 0; i < (m_support_wheel_count - 1); i++)
        {
            for (int j = 0; j < sup_tracks_count; j++)
            {
                sup_tracks_mass[i, j] = sup_direction[i].transform.GetChild(j).gameObject;
            }
        }


        //к зубчатому дирекшен

        from_sup_to_gear_direction = Support_wheel_mounts[m_support_wheel_count - 1].transform.GetChild(3).gameObject;



        //касательная от зубчатого
        from_gear_to_main_direction_rotation_point = Gear_wheel_mount.transform.GetChild(2).gameObject;
        from_gear_to_main_direction = from_gear_to_main_direction_rotation_point.transform.GetChild(0).gameObject;

        //от касательной к катку
        from_gear_to_main_napr_direction_offset_point = Main_wheel_mounts[0].transform.GetChild(9).gameObject;
        from_gear_to_main_napr_direction_rotation_point = from_gear_to_main_napr_direction_offset_point.transform.GetChild(0).gameObject;
        from_gear_to_main_napr_direction = from_gear_to_main_napr_direction_rotation_point.transform.GetChild(0).gameObject;
        from_gear_to_main_napr_direction_rotation_point_offset_proverka= from_gear_to_main_napr_direction_rotation_point.transform.GetChild(1).gameObject;
        //ЗУБЧАТОЕ
        gear_direction_rotation_point = Gear_wheel_mount.transform.GetChild(3).gameObject;


    }

    


    //void tracks_points()
    //{
  

    //}


    //void tracks_thickness()
    //{

    //}


    void rotate_points()
    {
        Vector3 from_main_to_free_direct = Main_wheel_shapes[m_wheel_count-1].transform.position - Free_wheel_mount.transform.position;
        angle_betwen_UP_and_main_to_free = Vector3.Angle(Up_direction, from_main_to_free_direct);

        Main_wheel_points[m_wheel_count - 1].transform.localEulerAngles = new Vector3(90f - angle_betwen_UP_and_main_to_free, 0, 0);
        Free_down_rotation_point.transform.localEulerAngles = new Vector3(180f-angle_betwen_UP_and_main_to_free, 0, 0);

        //Индусский код
        from_down_main_to_down_free_direct = First_main_offset_down_point.transform.position - Free_down_offset_point.transform.position;
        angle_betwen_UP_and_main_down_to_free_down = Vector3.Angle(Up_direction, from_down_main_to_down_free_direct);
        
        Free_angled_down_rotation_point.transform.localEulerAngles = new Vector3(180f - angle_betwen_UP_and_main_down_to_free_down, 0, 0);
        from_main_to_free_tracks_direction_vec = First_main_offset_down_point.transform.position - Free_angled_down_offset_point.transform.position;
        angle_betwen_UP_and_main_down_to_free_down_TWO = Vector3.Angle(Up_direction, from_main_to_free_tracks_direction_vec);
        First_main_angled_rotation_down_point.transform.localEulerAngles = new Vector3(90f - angle_betwen_UP_and_main_down_to_free_down_TWO, 0, 0);
        from_main_to_free_tracks_direction_vec3 = First_main_angled_rotation_down_point.transform.position - Free_angled_down_rotation_point.transform.position;
        angle_betwen_UP_and_main_down_to_free_down_THREE = Vector3.Angle(Up_direction, from_main_to_free_tracks_direction_vec3);

        if (m_support_wheel_count!=0)
        {
            Vector3 from_free_to_sup_direct = Free_wheel_mount.transform.position - Support_wheel_mounts[0].transform.position;
            angle_betwen_up_and_free_to_sup = Vector3.Angle(Up_direction, from_free_to_sup_direct);

            from_free_up_to_sup_up_direct = Free_up_offset_point.transform.position - First_support_up_point.transform.position;
            angle_betwen_up_and_free_up_to_sup_up = Vector3.Angle(Up_direction, from_free_up_to_sup_up_direct);


            Vector3 from_sup_to_gear_direct = Support_wheel_mounts[m_support_wheel_count-1].transform.position - Gear_wheel_mount.transform.position;
            angle_betwen_up_and_sup_to_gear = Vector3.Angle(Up_direction, from_sup_to_gear_direct);

            from_sup_up_to_gear_up_direct = Last_support_up_point.transform.position - Gear_up_offset_point.transform.position;
            angle_betwen_up_and_sup_up_to_gear_up = Vector3.Angle(Up_direction, from_sup_up_to_gear_up_direct);
            //Vector3 from_sup_up_to_gear_up_direct_true = from_sup_to_gear_direction.transform.position - Gear_up_offset_point.transform.position;


        }
        Free_up_rotation_point.transform.localEulerAngles = new Vector3(180f + angle_betwen_up_and_free_up_to_sup_up, 0, 0);

        Gear_up_rotation_point.transform.localEulerAngles = new Vector3(180f + angle_betwen_up_and_sup_to_gear, 0, 0);


        Vector3 from_gear_to_main_direct = Gear_wheel_mount.transform.position - Main_wheel_shapes[0].transform.position;
        angle_betwen_UP_and_gear_to_main = Vector3.Angle(Up_direction, from_gear_to_main_direct);

        from_down_gear_to_down_main_direct = Gear_down_offset_point.transform.position - Last_main_offset_down_point.transform.position;
        angle_betwen_UP_and_gear_down_to_main_down = Vector3.Angle(Up_direction, from_down_gear_to_down_main_direct);

        Gear_down_rotation_point.transform.localEulerAngles = new Vector3(180f - angle_betwen_UP_and_gear_to_main, 0, 0);
        Last_main_rotation_point.transform.localEulerAngles = new Vector3(90f - angle_betwen_UP_and_gear_to_main, 0, 0);

    }

    void offset_points()
    {
        First_main_offset_down_point.transform.localPosition = new Vector3(0, -m_main_wheel_radius, 0);
        First_main_angled_offset_down_point.transform.localPosition = new Vector3(0, -m_main_wheel_radius, 0);

        


        Last_main_offset_down_point.transform.localPosition = new Vector3(0, -m_main_wheel_radius, 0);
        Last_main_offset_down_point_angle_0.transform.localPosition = new Vector3(0, -m_main_wheel_radius, 0);

        Free_down_offset_point.transform.localPosition = new Vector3(0, 0, m_free_wheel_radius);
        Free_up_offset_point.transform.localPosition = new Vector3(0, 0, m_free_wheel_radius);
        Free_angled_down_offset_point.transform.localPosition = new Vector3(0, 0, m_free_wheel_radius+track_high);


        First_support_up_point.transform.localPosition = new Vector3(0, m_support_wheel_radius, 0);
        Last_support_up_point.transform.localPosition = new Vector3(0, m_support_wheel_radius, 0);

        Gear_up_offset_point.transform.localPosition = new Vector3(0,0,m_gear_wheel_radius);
        Gear_down_offset_point.transform.localPosition = new Vector3(0, 0, m_gear_wheel_radius);


        free_wheel_tracks_sign_point.transform.localPosition = new Vector3(0, 0, m_free_wheel_radius + track_high);
    }

    void get_points()
    {
        First_main_offset_down_point = Main_wheel_mounts[m_wheel_count - 1].transform.GetChild(3).GetChild(0).gameObject;
        First_main_offset_down_point_angle_0 = Main_wheel_mounts[m_wheel_count - 1].transform.GetChild(10).gameObject;
        Last_main_rotation_point = Main_wheel_mounts[0].transform.GetChild(3).gameObject;
        Last_main_offset_down_point = Main_wheel_mounts[0].transform.GetChild(3).GetChild(0).gameObject;
        Last_main_offset_down_point_angle_0 = Main_wheel_mounts[0].transform.GetChild(10).gameObject;
        Free_down_rotation_point = Free_wheel_mount.transform.GetChild(1).GetChild(0).gameObject;
        Free_down_offset_point = Free_down_rotation_point.transform.GetChild(0).gameObject;

        Free_angled_down_rotation_point = Free_wheel_mount.transform.GetChild(4).gameObject;
        Free_angled_down_offset_point = Free_angled_down_rotation_point.transform.GetChild(0).gameObject;

        First_main_angled_rotation_down_point = Main_wheel_mounts[m_wheel_count - 1].transform.GetChild(11).gameObject;
        First_main_angled_offset_down_point = First_main_angled_rotation_down_point.transform.GetChild(0).gameObject;

        Free_up_rotation_point = Free_wheel_mount.transform.GetChild(1).GetChild(1).gameObject;
        Free_up_offset_point = Free_up_rotation_point.transform.GetChild(0).gameObject;

        if (m_support_wheel_count != 0)
        {
            First_support_up_point = Support_wheel_mounts[0].transform.GetChild(1).GetChild(0).gameObject;
            Last_support_up_point = Support_wheel_mounts[m_support_wheel_count-1].transform.GetChild(1).GetChild(0).gameObject;
        }

        Gear_up_rotation_point = Gear_wheel_mount.transform.GetChild(1).GetChild(0).gameObject;
        Gear_up_offset_point = Gear_up_rotation_point.transform.GetChild(0).gameObject;


        Gear_down_rotation_point = Gear_wheel_mount.transform.GetChild(1).GetChild(1).gameObject;
        Gear_down_offset_point = Gear_down_rotation_point.transform.GetChild(0).gameObject;

    }


    void Tracks_start()
    {
        get_tracks_directions();
        get_points();
        offset_points();
    }

    void Up_dir()
    {
        Up_direction = Up_obj.transform.position - Down_obj.transform.position;
    }

    void Tracks_update()
    {

        
        rotate_points();
        #region main
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //ОПРЕДЕЛЕНИЯ ВЕКТОРОВ ГЛАВНЫХ КОЛЕС
        //вектора-от первого колеса ко второму, от второго к третьему и т.д.
        Vector3[] wheels_vec = new Vector3[m_wheel_count - 1];
        for (int i = 0; i < (m_wheel_count - 1); i++)
        {
            wheels_vec[i] = Main_wheel_shapes[i + 1].transform.position - Main_wheel_shapes[i].transform.position;
        }

        float[] wheels_vec_angles = new float[m_wheel_count - 1];
        for (int i = 0; i < (m_wheel_count - 1); i++)
        {
            wheels_vec_angles[i] = 90f - Vector3.Angle(Up_direction, wheels_vec[i]);
        }
        //скорость катков, одна для всех
        tracks_position_speed += -z_speed_all / 100f * Time.deltaTime * 60f;
        if(count_grounded_wheels() < 1)
        {
            tracks_position_speed = 0f;
        }
        //НАПРАВЛЕНИЯ ДИРЕКШЕНОВ ГЛАВНЫХ КАТКОВ
        for (int i = 0; i < (m_wheel_count - 1); i++)
        {
            track_direction[i].transform.localEulerAngles = new Vector3(-wheels_vec_angles[i], 0, 0);
            track_direction[i].transform.localPosition = new Vector3(0, Main_wheel_shapes[i].transform.localPosition.y - m_main_wheel_radius - track_high, 0);
            track_direction[i].transform.localScale = new Vector3(1, 1, 1 * wheels_vec[i].magnitude / m_Main_wheel_spacing);
            //в конкретном данном случае вместо корректировки времени отдельных траков лучше изменить длину директа
            //т.к. кривая одна единственная. и изменить ее для каждого члена массива не удасться

        }


        //РАБОТА С КРИВЫМИ ТРАКОВ ГЛАВНЫХ КАТКОВ
        float shag = m_Main_wheel_spacing / (float)main_tracks_count;
        main_tracks_position_curve.postWrapMode = WrapMode.Loop;
        main_tracks_position_curve.preWrapMode = WrapMode.Loop;
        Keyframe[] main_keys_of_curve = main_tracks_position_curve.keys;
        main_keys_of_curve[0].value = 0;
        main_keys_of_curve[0].time = 0;
        main_keys_of_curve[1].value = m_Main_wheel_spacing;
        main_keys_of_curve[1].time = m_Main_wheel_spacing;
        main_tracks_position_curve.keys = main_keys_of_curve;
        
        for (int i = 0; i < (m_wheel_count - 1); i++)
        {
            for (int j = 0; j < main_tracks_count; j++)
            {
                main_tracks_mass[i, j].transform.localPosition = new Vector3(0f, 0f, main_tracks_position_curve.Evaluate(shag * j + tracks_position_speed));
                main_tracks_mass[i, j].transform.localScale = new Vector3(1 * m_tracks_width, 1, 1 * (shag / track_part_length));
            }
        }
        
        Transform[] main_target = new Transform[(m_wheel_count - 2)];
        for (int i = 0; i < (m_wheel_count - 2); i++)
        {
            for (int j = 0; j < main_tracks_count; j++)
            {
                GameObject current_track = main_tracks_mass[i + 1, j];
                if (current_track.transform.localPosition.z < (shag))
                {
                    main_target[i] = current_track.transform;

                }
            }
        }
        
        for (int i = 0; i < (m_wheel_count - 2); i++)
        {
            for (int j = 0; j < main_tracks_count; j++)
            {
                GameObject current_track = main_tracks_mass[i, j];
                if (current_track.transform.localPosition.z > (m_Main_wheel_spacing - shag))
                {
                    current_track.transform.LookAt(main_target[i], Up_direction);
                    

                }
                else
                {
                    current_track.transform.localEulerAngles = new Vector3(0, 0, 0);
                }
            }
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion
        //---------------------------------------------------------------------------------------------------------------------
        #region free_rotation_direction
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //НАПРАВЛЕНИЯ ГЛАВНОГО ДИРЕКШЕНА ПЕРЕДНЕЙ ВРАЩАТЕЛЬНОЙ
        free_direction_rotation_point.transform.localEulerAngles = new Vector3(90f - angle_betwen_UP_and_main_down_to_free_down, 0, 0);
        //РАБОТА С КРИВЫМИ ТРАКОВ ПЕРЕДНЕЙ ВРАЩАТЕЛЬНОЙ
        Vector3 free_center_to_up = Free_up_offset_point.transform.position - Free_wheel_mount.transform.position;
        Vector3 free_center_do_down = Free_down_offset_point.transform.position - Free_wheel_mount.transform.position;
        float free_tracks_direction_angled_length = Vector3.Angle(free_center_to_up, free_center_do_down);
        float free_tracks_direction_length = Mathf.PI * (m_free_wheel_radius + track_high) * free_tracks_direction_angled_length / 180f;

        int free_tracks_count = Mathf.FloorToInt(free_tracks_direction_length / track_part_length) + 1;

        float free_shag_full = free_tracks_direction_length / ((float)free_tracks_count - 1);
        float free_angled_shag_full = free_tracks_direction_angled_length / ((float)free_tracks_count - 1);
        float free_shag_not_full = free_tracks_direction_length / ((float)free_tracks_count);

        free_tracks_position_curve.postWrapMode = WrapMode.Loop;
        free_tracks_position_curve.preWrapMode = WrapMode.Loop;

        Keyframe[] free__curve = free_tracks_position_curve.keys;
        free__curve[0].value = 0;
        free__curve[0].time = 0;
        free__curve[1].value = free_tracks_direction_length;//free_tracks_direction_length
        free__curve[1].time = free_tracks_direction_length;
        free_tracks_position_curve.keys = free__curve;

        for (int i = 0; i < free_direction_rotation_point.transform.childCount; i++)
        {
            GameObject current_rot_dir_obj = free_direction_rotation_point.transform.GetChild(i).gameObject;
            current_rot_dir_obj.SetActive(false);
        }

        for (int i = 0; i < free_tracks_count; i++)
        {
            GameObject current_rot_dir_obj = free_direction_rotation_point.transform.GetChild(i).gameObject;
            current_rot_dir_obj.SetActive(true);

            Transform current_rot_dir = current_rot_dir_obj.transform;
            current_rot_dir.localEulerAngles = new Vector3(free_tracks_direction_angled_length / free_tracks_direction_length * -free_tracks_position_curve.Evaluate(free_shag_full * i + tracks_position_speed * free_shag_full / shag), 0f, 0f);
            current_rot_dir.GetChild(0).localPosition = new Vector3(0, -m_free_wheel_radius - track_high, 0);
            current_rot_dir.GetChild(0).localScale = new Vector3(1 * m_tracks_width, 1, 1 * (free_shag_full / track_part_length));


            Transform indicator = current_rot_dir.GetChild(0).GetChild(2);
            indicator.transform.localPosition = new Vector3(0, 0, free_tracks_position_curve.Evaluate(free_shag_full * i + tracks_position_speed * free_shag_full / shag));



        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        //скрепляющие звенья


        #endregion
        //---------------------------------------------------------------------------------------------------------------------
        #region from_main_to_free
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //НАПРАВЛЕНИЯ ДИРЕКШЕНОВ ПЕРЕДНЕЙ КАСАТЕЛЬНОЙ
        Vector3 from_main_to_free_length_vec = Free_angled_down_offset_point.transform.position - from_main_to_free_direction.transform.position;
        float from_main_to_free_tracks_direction_length = from_main_to_free_length_vec.magnitude - track_part_length * free_shag_full / 2f;
        int from_main_to_free_tracks_count = Mathf.FloorToInt(from_main_to_free_tracks_direction_length / track_part_length);
        float from_main_to_free_shag = from_main_to_free_tracks_direction_length / (float)from_main_to_free_tracks_count;

        from_main_to_free_direction_rotation_point.transform.localEulerAngles = new Vector3(90f - angle_betwen_UP_and_main_down_to_free_down_TWO, 0, 0);
        from_main_to_free_direction_rotation_point.transform.localPosition = new Vector3(0, Main_wheel_shapes[m_wheel_count - 1].transform.localPosition.y, 0);
        from_main_to_free_direction.transform.localPosition = new Vector3(0, -m_main_wheel_radius - track_high, 0);
        from_main_to_free_direction.transform.LookAt(Free_angled_down_offset_point.transform, Up_direction);
        
        
        //РАБОТА С КРИВЫМИ ТРАКОВ ПЕРЕДНЕЙ КАСАТЕЛЬНОЙ
        from_main_to_free_tracks_position_curve.postWrapMode = WrapMode.Loop;
        from_main_to_free_tracks_position_curve.preWrapMode = WrapMode.Loop;
        Keyframe[] main_keys_of_from_mait_to_free__curve = from_main_to_free_tracks_position_curve.keys;
        main_keys_of_from_mait_to_free__curve[0].value = 0;
        main_keys_of_from_mait_to_free__curve[0].time = 0;

        main_keys_of_from_mait_to_free__curve[1].value = from_main_to_free_tracks_direction_length;
        main_keys_of_from_mait_to_free__curve[1].time = from_main_to_free_tracks_direction_length;
        from_main_to_free_tracks_position_curve.keys = main_keys_of_from_mait_to_free__curve;

        for (int i = 0; i < from_main_to_free_direction.transform.childCount; i++)
        {
            GameObject current_track_obj = from_main_to_free_direction.transform.GetChild(i).gameObject;
            current_track_obj.SetActive(false);
        }

        for (int i = 0; i < from_main_to_free_tracks_count; i++)
        {
            GameObject current_track_obj = from_main_to_free_direction.transform.GetChild(i).gameObject;
            current_track_obj.SetActive(true);

            Transform current_track = current_track_obj.transform;

            current_track.localPosition = new Vector3(0f, 0f, from_main_to_free_tracks_position_curve.Evaluate(from_main_to_free_shag * i + tracks_position_speed * from_main_to_free_shag / shag));
            current_track.localScale = new Vector3(1 * m_tracks_width, 1, 1 * (from_main_to_free_shag / track_part_length));
        }
        //1578 ТУТ
        //ОПРЕДЕЛЕНИЕ ЦЕЛИ КАСАТЕЛЬНЫХ КАТКОВ

        Transform from_main_to_free_tracks_target = null;
        for (int i = 0; i < free_tracks_count; i++)
        {
            GameObject current_rot_dir_obj = free_direction_rotation_point.transform.GetChild(i).gameObject;
            Transform current_rot_dir = current_rot_dir_obj.transform;
            //
            Transform indicator = current_rot_dir.GetChild(0).GetChild(2);
           
           //
            if (indicator.localPosition.z < 0.4f)
            
            {
                from_main_to_free_tracks_target = current_rot_dir.GetChild(0).GetChild(0);
            }
            else
                from_main_to_free_tracks_target = null;
        }

        //КАСАТЕЛЬНЫЙ ТРАК СМОТРИТ В ЦЕЛЬ
        for (int i = 0; i < from_main_to_free_tracks_count; i++)
        {
            Transform current_track = from_main_to_free_direction.transform.GetChild(i);
            if (current_track.transform.localPosition.z > (main_keys_of_from_mait_to_free__curve[1].value - from_main_to_free_shag) && from_main_to_free_tracks_target != null)
            {
                //current_track.LookAt(from_main_to_free_tracks_target, Up_direction);
               
            }
            else
            {
                current_track.localEulerAngles = new Vector3(0, 0, 0);
            }
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion
        //---------------------------------------------------------------------------------------------------------------------
        #region first_main_to_kasateln
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //НАПРАВЛЕНИЕ ПЕРЕДНЕГО  ДИРЕКШЕНА ОСНОВНОГО ПЕРЕДНЕГО КАТКА
        main_direction_front_rotation_point.transform.localPosition = new Vector3(0, Main_wheel_shapes[m_wheel_count - 1].transform.localPosition.y - m_main_wheel_radius - track_high, 0);
        main_direction_front_direction.transform.LookAt(from_main_to_free_direction.transform, Up_direction);
        
        //РАБОТА С КРИВЫМИ  ПЕРЕДНЕГО  ДИРЕКШЕНА ОСНОВНОГО ПЕРЕДНЕГО КАТКА
        Vector3 from_main_to_kos_tracks_direction_vec = from_main_to_free_direction.transform.position - main_direction_front_direction.transform.position;
        float from_main_to_kos_tracks_direction_length = from_main_to_kos_tracks_direction_vec.magnitude;

        int from_main_to_kos_tracks_count = Mathf.Clamp(Mathf.FloorToInt(from_main_to_kos_tracks_direction_length / track_part_length), 1, 100);

        float from_main_to_kos_shag = from_main_to_kos_tracks_direction_length / (float)from_main_to_kos_tracks_count;

        angled_front_tracks_position_curve.postWrapMode = WrapMode.Loop;
        angled_front_tracks_position_curve.preWrapMode = WrapMode.Loop;
        Keyframe[] main_keys_of_from_main_to_kos_curve = angled_front_tracks_position_curve.keys;
        main_keys_of_from_main_to_kos_curve[0].value = 0;
        main_keys_of_from_main_to_kos_curve[0].time = 0;
        main_keys_of_from_main_to_kos_curve[1].value = from_main_to_kos_tracks_direction_length;
        main_keys_of_from_main_to_kos_curve[1].time = from_main_to_kos_tracks_direction_length;
        angled_front_tracks_position_curve.keys = main_keys_of_from_main_to_kos_curve;

        for (int i = 0; i < main_direction_front_direction.transform.childCount; i++)
        {
            GameObject current_rot_dir_obj = main_direction_front_direction.transform.GetChild(i).gameObject;
            current_rot_dir_obj.SetActive(false);
        }

        for (int i = 0; i < from_main_to_kos_tracks_count; i++)
        {
            GameObject current_rot_dir_obj = main_direction_front_direction.transform.GetChild(i).gameObject;
            current_rot_dir_obj.SetActive(true);

            Transform current_rot_dir = current_rot_dir_obj.transform;
            current_rot_dir.transform.localPosition = new Vector3(0f, 0f, angled_front_tracks_position_curve.Evaluate(from_main_to_kos_shag * i + tracks_position_speed * from_main_to_kos_shag / shag));
            current_rot_dir.transform.localScale = new Vector3(1 * m_tracks_width, 1, 1 * (from_main_to_kos_shag / track_part_length));
        }
        //ОПРЕДЕЛЕНИЕ ЦЕЛИ ОСНОВНЫХ ТРАКОВ ВТОРОГО ОПОРНОГО КАТКА
        Transform second_main_tracks_target = null;
        for (int j = 0; j < from_main_to_kos_tracks_count; j++)
        {
            GameObject current_track = main_direction_front_direction.transform.GetChild(j).gameObject;
            if (current_track.transform.localPosition.z < from_main_to_kos_shag )
            {
                second_main_tracks_target = current_track.transform.transform;
            }
        }
        //СМОТРЯТ В ЦЕЛЬ
        for (int i = 0; i < main_tracks_count; i++)
        {
            GameObject current_rot_dir_obj = main_tracks_mass[m_wheel_count-2, i].gameObject;
            Transform current_rot_dir = current_rot_dir_obj.transform;
            if (current_rot_dir.localPosition.z > (m_Main_wheel_spacing - shag))
            {
                current_rot_dir.LookAt(second_main_tracks_target, Up_direction);
                
            }
            else
            {
                current_rot_dir.localEulerAngles = new Vector3(0, 0, 0);
            }
        }

        //ОПРЕДЕЛЕНИЕ ЦЕЛИ ТРАКОВ ДИРЕКШЕНА ПЕРЕДНЕГО ОПОРНОГО КАТКА
        Transform angled_front_tracks_target = null;
        for (int j = 0; j < from_main_to_free_tracks_count; j++)
        {
            GameObject current_track = from_main_to_free_direction.transform.GetChild(j).gameObject;
            if (current_track.transform.localPosition.z < from_main_to_free_shag)
            {
                angled_front_tracks_target = current_track.transform.transform;
            }
        }
        //СМОТРЯТ В ЦЕЛЬ
        for (int i = 0; i < from_main_to_kos_tracks_count; i++)
        {
            GameObject current_rot_dir_obj = main_direction_front_direction.transform.GetChild(i).gameObject;
            Transform current_rot_dir = current_rot_dir_obj.transform;
            if (current_rot_dir.localPosition.z > (from_main_to_kos_tracks_direction_length - from_main_to_kos_shag))
            {
                current_rot_dir.LookAt(angled_front_tracks_target, Up_direction);
                
            }
            else
            {
                current_rot_dir.localEulerAngles = new Vector3(0, 0, 0);
            }
        }

        //Если расстояние Angled Front траков слишком маленькое
        if(from_main_to_kos_tracks_direction_length < minimal_track_length)
        {
            //Выключаем лишнее, так как короткое
            for (int i = 0; i < main_direction_front_direction.transform.childCount; i++)
            {
                GameObject current_rot_dir_obj = main_direction_front_direction.transform.GetChild(i).gameObject;
                current_rot_dir_obj.SetActive(false);
            }
            //Поворачиваем касательную////
            from_main_to_free_direction_rotation_point.transform.localEulerAngles = new Vector3(0, 0, 0);
            from_main_to_free_direction.transform.LookAt(Free_angled_down_offset_point.transform, Up_direction);
            
            //Определение цели переднего трака основных катков 
            Transform main_track_target = null;
            for (int j = 0; j < from_main_to_free_tracks_count; j++)
            {
                GameObject current_track = from_main_to_free_direction.transform.GetChild(j).gameObject;
                if (current_track.transform.localPosition.z < from_main_to_free_shag)
                {
                    main_track_target = current_track.transform.transform;
                }
            }
            //СМОТРЯТ В ЦЕЛЬ
            for (int i = 0; i < main_tracks_count; i++)
            {
                GameObject current_rot_dir_obj = main_tracks_mass[m_wheel_count - 2, i].gameObject;
                Transform current_rot_dir = current_rot_dir_obj.transform;
                if (current_rot_dir.localPosition.z > (m_Main_wheel_spacing - shag))
                {
                    current_rot_dir.LookAt(main_track_target, Up_direction);
                   
                }
                else
                {
                    current_rot_dir.localEulerAngles = new Vector3(0, 0, 0);
                }
            }
            #region from_main_to_free_again 
            //Почему то с пересчетом работает хуже
            ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            //НАПРАВЛЕНИЯ ДИРЕКШЕНОВ ПЕРЕДНЕЙ КАСАТЕЛЬНОЙ
            //Позиция from_main_to_free_direction изменилась, пересчитываем
            from_main_to_free_length_vec = Free_angled_down_offset_point.transform.position - from_main_to_free_direction.transform.position;
            from_main_to_free_tracks_direction_length = from_main_to_free_length_vec.magnitude - track_part_length * free_shag_full / 2f;
            from_main_to_free_tracks_count = Mathf.FloorToInt(from_main_to_free_tracks_direction_length / track_part_length);
            from_main_to_free_shag = from_main_to_free_tracks_direction_length / (float)from_main_to_free_tracks_count;

            //РАБОТА С КРИВЫМИ ТРАКОВ ПЕРЕДНЕЙ КАСАТЕЛЬНОЙ
            from_main_to_free_tracks_position_curve.postWrapMode = WrapMode.Loop;
            from_main_to_free_tracks_position_curve.preWrapMode = WrapMode.Loop;
            main_keys_of_from_mait_to_free__curve = from_main_to_free_tracks_position_curve.keys;
            main_keys_of_from_mait_to_free__curve[0].value = 0;
            main_keys_of_from_mait_to_free__curve[0].time = 0;

            main_keys_of_from_mait_to_free__curve[1].value = from_main_to_free_tracks_direction_length;
            main_keys_of_from_mait_to_free__curve[1].time = from_main_to_free_tracks_direction_length;
            from_main_to_free_tracks_position_curve.keys = main_keys_of_from_mait_to_free__curve;

            for (int i = 0; i < from_main_to_free_direction.transform.childCount; i++)
            {
                GameObject current_track_obj = from_main_to_free_direction.transform.GetChild(i).gameObject;
                current_track_obj.SetActive(false);
            }

            for (int i = 0; i < from_main_to_free_tracks_count; i++)
            {
                GameObject current_track_obj = from_main_to_free_direction.transform.GetChild(i).gameObject;
                current_track_obj.SetActive(true);

                Transform current_track = current_track_obj.transform;

                current_track.localPosition = new Vector3(0f, 0f, from_main_to_free_tracks_position_curve.Evaluate(from_main_to_free_shag * i + tracks_position_speed * from_main_to_free_shag / shag));
                current_track.localScale = new Vector3(1 * m_tracks_width, 1, 1 * (from_main_to_free_shag / track_part_length));
            }
            #endregion

        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion
        //---------------------------------------------------------------------------------------------------------------------
        #region from_free_to_sup
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //НАПРАВЛЕНИЯ ДИРЕКШЕНОВ ОТ ЛЕНИВЦА К ПЕРВОМУ ПОДДЕРЖИВАЮЩЕМУ
        from_free_to_sup_direction_rotation_point.transform.localEulerAngles = new Vector3(270f + angle_betwen_up_and_free_up_to_sup_up, 0, 0);
        from_free_to_sup_direction.transform.localEulerAngles = new Vector3(180f, 0f, 0f);
        from_free_to_sup_direction.transform.localPosition = new Vector3(0, m_free_wheel_radius + track_high, 0);

        //РАБОТА С КРИВЫМИ ОТ ЛЕНИВЦА К ПЕРВОМУ ПОДЕРЖИВАЮЩЕМУ
        //float from_free_to_sup_tracks_direction_length = from_free_up_to_sup_up_direct.magnitude + track_part_length * free_shag_full / 2f;
        Vector3 from_free_to_sup_tracks_direction_length_vec = sup_direction[0].transform.position - from_free_to_sup_direction.transform.position;
 
        float from_free_to_sup_tracks_direction_length = from_free_up_to_sup_up_direct.magnitude;
        int from_free_to_sup_tracks_count = Mathf.Clamp(Mathf.FloorToInt(from_free_to_sup_tracks_direction_length / track_part_length), 1, 100);

        float from_free_to_sup_shag = from_free_to_sup_tracks_direction_length / (float)from_free_to_sup_tracks_count;

        from_free_to_sup_tracks_position_curve.postWrapMode = WrapMode.Loop;
        from_free_to_sup_tracks_position_curve.preWrapMode = WrapMode.Loop;
        Keyframe[] main_keys_of_from_free_to_sup_curve = from_free_to_sup_tracks_position_curve.keys;
        main_keys_of_from_free_to_sup_curve[0].value = 0f;//-track_part_length * free_shag_full / 2f;
        main_keys_of_from_free_to_sup_curve[0].time = 0f;//-track_part_length * free_shag_full / 2f;
        main_keys_of_from_free_to_sup_curve[1].value = from_free_to_sup_tracks_direction_length;//from_free_to_sup_tracks_direction_length - track_part_length * free_shag_full / 2f;
        main_keys_of_from_free_to_sup_curve[1].time = from_free_to_sup_tracks_direction_length;//from_free_to_sup_tracks_direction_length - track_part_length * free_shag_full / 2f;
        from_free_to_sup_tracks_position_curve.keys = main_keys_of_from_free_to_sup_curve;

        for (int i = 0; i < from_free_to_sup_direction.transform.childCount; i++)
        {
            GameObject current_rot_dir_obj = from_free_to_sup_direction.transform.GetChild(i).gameObject;
            current_rot_dir_obj.SetActive(false);
        }

        for (int i = 0; i < from_free_to_sup_tracks_count; i++)
        {
            GameObject current_rot_dir_obj = from_free_to_sup_direction.transform.GetChild(i).gameObject;
            current_rot_dir_obj.SetActive(true);

            Transform current_rot_dir = current_rot_dir_obj.transform;
            current_rot_dir.transform.localPosition = new Vector3(0f, 0f, from_free_to_sup_tracks_position_curve.Evaluate(from_free_to_sup_shag * i + tracks_position_speed * from_free_to_sup_shag / shag));
            current_rot_dir.transform.localScale = new Vector3(1 * m_tracks_width, 1, 1 * (from_free_to_sup_shag / track_part_length));
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion
        //---------------------------------------------------------------------------------------------------------------------
        #region main_support
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //НАПРАВЛЕНИЯ ДИРЕКШЕНОВ ПОДДЕРЖИВАЮЩИХ
        for (int i = 0; i < (m_support_wheel_count - 1); i++)
        {
            sup_direction[i].transform.localEulerAngles = new Vector3(180f, 0, 0);
            sup_direction[i].transform.localPosition = new Vector3(0, m_support_wheel_radius + track_high, 0);
            sup_direction[i].transform.localScale = new Vector3(1, 1, 1);
            //в конкретном данном случае вместо корректировки времени отдельных траков лучше изменить длину директа
            //т.к. кривая одна единственная. и изменить ее для каждого члена массива не удасться

        }

        //РАБОТА С КРИВЫМИ ПОДДЕРЖИВАЮЩИХ
        float sup_tracks_direction_length = m_Support_wheel_spacing;

        float sup_shag = sup_tracks_direction_length / (float)sup_tracks_count;

        sup_tracks_position_curve.postWrapMode = WrapMode.Loop;
        sup_tracks_position_curve.preWrapMode = WrapMode.Loop;
        Keyframe[] to_sup_curve = sup_tracks_position_curve.keys;
        to_sup_curve[0].value = 0;
        to_sup_curve[0].time = 0;
        to_sup_curve[1].value = m_Support_wheel_spacing;
        to_sup_curve[1].time = m_Support_wheel_spacing;
        sup_tracks_position_curve.keys = to_sup_curve;


        for (int i = 0; i < (m_support_wheel_count - 1); i++)
        {
            for (int j = 0; j < sup_direction[i].transform.childCount; j++)
            {
                GameObject current_track = sup_direction[i].transform.GetChild(j).gameObject;
                current_track.SetActive(false);
            }
        }

        for (int i = 0; i < (m_support_wheel_count - 1); i++)
        {
            for (int j = 0; j < sup_direction[i].transform.childCount; j++)
            {
                GameObject current_track = sup_direction[i].transform.GetChild(j).gameObject;
                current_track.SetActive(true);

                current_track.transform.localPosition = new Vector3(0f, 0f, sup_tracks_position_curve.Evaluate(sup_shag * j + tracks_position_speed * sup_shag / shag));
                current_track.transform.localScale = new Vector3(1 * m_tracks_width, 1, 1 * (sup_shag / track_part_length));
            }
        }
        //ОПРЕДЕЛЕНИЕ ЦЕЛИ ОТ ЛЕНИВЦА К ПОДДЕРЖИВАЮЩЕМУ
        Transform from_free_to_sup_tracks_target = null;
        for (int j = 0; j < sup_direction[0].transform.childCount; j++)
        {
            GameObject current_track = sup_direction[0].transform.GetChild(j).gameObject;
            if (current_track.transform.localPosition.z < sup_shag )
            {
                from_free_to_sup_tracks_target = current_track.transform.transform;
            }
        }

        //ЛЕНИВЕЦ-ПОДДЕРЖИВАЮЩИЙ ТРАКИ СМОТРЯТ В ЦЕЛЬ
        for (int i = 0; i < from_free_to_sup_tracks_count; i++)
        {
            GameObject current_rot_dir_obj = from_free_to_sup_direction.transform.GetChild(i).gameObject;
            Transform current_rot_dir = current_rot_dir_obj.transform;
            if (current_rot_dir.localPosition.z > (main_keys_of_from_free_to_sup_curve[1].time - from_free_to_sup_shag))
            {
                current_rot_dir.LookAt(from_free_to_sup_tracks_target,Up_direction*-1f);
               
               
            }
            else
            {
                current_rot_dir.localEulerAngles = new Vector3(0, 0, 0);
            }
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion
        //---------------------------------------------------------------------------------------------------------------------
        #region from_sup_to_gear
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //НАПРАВЛЕНИЯ ДИРЕКШЕНОВ ОТ ПОДДЕРЖИВАЮЩЕГО К ЗУБЧАТОМУ
        from_sup_to_gear_direction.transform.localEulerAngles = new Vector3(270f + angle_betwen_up_and_sup_up_to_gear_up + 180f, 0, 0);
        from_sup_to_gear_direction.transform.localPosition = new Vector3(0, m_support_wheel_radius + track_high, 0);


        //РАБОТА С КРИВЫМИ ОТ ЛЕНИВЦА К ПЕРВОМУ ПОДЕРЖИВАЮЩЕМУ

        float from_sup_to_gear_tracks_direction_length = from_sup_up_to_gear_up_direct.magnitude;

        int from_sup_to_gear_tracks_count = Mathf.Clamp(Mathf.FloorToInt(from_sup_to_gear_tracks_direction_length / track_part_length), 1, 100);

        float from_sup_to_gear_shag = from_sup_to_gear_tracks_direction_length / (float)from_sup_to_gear_tracks_count;

        from_sup_to_gear_tracks_position_curve.postWrapMode = WrapMode.Loop;
        from_sup_to_gear_tracks_position_curve.preWrapMode = WrapMode.Loop;
        Keyframe[] main_keys_of_from_sup_to_gear_curve = from_sup_to_gear_tracks_position_curve.keys;
        main_keys_of_from_sup_to_gear_curve[0].value = 0;
        main_keys_of_from_sup_to_gear_curve[0].time = 0;
        main_keys_of_from_sup_to_gear_curve[1].value = from_sup_to_gear_tracks_direction_length;
        main_keys_of_from_sup_to_gear_curve[1].time = from_sup_to_gear_tracks_direction_length;
        from_sup_to_gear_tracks_position_curve.keys = main_keys_of_from_sup_to_gear_curve;


        for (int i = 0; i < from_sup_to_gear_direction.transform.childCount; i++)
        {
            GameObject current_rot_dir_obj = from_sup_to_gear_direction.transform.GetChild(i).gameObject;
            current_rot_dir_obj.SetActive(false);
        }

        for (int i = 0; i < from_sup_to_gear_tracks_count; i++)
        {
            GameObject current_rot_dir_obj = from_sup_to_gear_direction.transform.GetChild(i).gameObject;
            current_rot_dir_obj.SetActive(true);

            Transform current_rot_dir = current_rot_dir_obj.transform;
            current_rot_dir.localPosition = new Vector3(0f, 0f, from_sup_to_gear_tracks_position_curve.Evaluate(from_sup_to_gear_shag * i + tracks_position_speed * from_sup_to_gear_shag / shag));
            current_rot_dir.localScale = new Vector3(1 * m_tracks_width, 1, 1 * (from_sup_to_gear_shag / track_part_length));
        }


        //ОПРЕДЕЛЕНИЕ ЦЕЛИ ПОСЛЕДНЕГО ТРАКА ПОДДЕРЖИВАЮЩИХ 
        Transform sup_tracks_target = null;
        for (int i = 0; i < from_sup_to_gear_tracks_count; i++)
        {
            GameObject current_rot_dir_obj = from_sup_to_gear_direction.transform.GetChild(i).gameObject;
            if (current_rot_dir_obj.transform.localPosition.z < from_sup_to_gear_shag)
            {
                sup_tracks_target = current_rot_dir_obj.transform.transform;
            }
        }
        //ПОСЛЕДНИЙ ПОДДЕРЖИВАЮЩИЙ ТРАКИ СМОТРЯТ В ЦЕЛЬ
        for (int j = 0; j < sup_tracks_count; j++)
        {
            GameObject current_track = sup_direction[m_support_wheel_count - 2].transform.GetChild(j).gameObject;
            if (current_track.transform.localPosition.z > (sup_tracks_direction_length - sup_shag))
            {
                current_track.transform.LookAt(sup_tracks_target, Up_direction * -1f);
                

            }
            else
            {
                current_track.transform.localEulerAngles = new Vector3(0, 0, 0);
            }
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion
        //---------------------------------------------------------------------------------------------------------------------
        #region zubchatoe_vrash
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //ЗУБЧАТОЕ
        gear_direction_rotation_point.transform.localEulerAngles = new Vector3(180f + angle_betwen_up_and_sup_to_gear, 0, 0);


        //РАБОТА С КРИВЫМИ ЗУБЧАТОГО
        Vector3 gear_center_to_up = Gear_wheel_mount.transform.position - Gear_up_offset_point.transform.position;
        Vector3 gear_center_to_down = Gear_wheel_mount.transform.position - Gear_down_offset_point.transform.position;

        float gear_direction_angled_length = Vector3.Angle(gear_center_to_up, gear_center_to_down);
        float gear_direction_length = Mathf.PI * (m_gear_wheel_radius + track_high) * gear_direction_angled_length / 180f;

        int gear_tracks_count = Mathf.Clamp(Mathf.FloorToInt(gear_direction_length / track_part_length) + 1, 1, 100);

        float gear_shag_full = gear_direction_length / ((float)gear_tracks_count - 1);
        float gear_angled_shag_full = gear_direction_angled_length / ((float)gear_tracks_count - 1);
        float gear_shag_not_full = gear_direction_length / ((float)gear_tracks_count);

        gear_tracks_position_curve.postWrapMode = WrapMode.Loop;
        gear_tracks_position_curve.preWrapMode = WrapMode.Loop;
        Keyframe[] gear_curve = gear_tracks_position_curve.keys;
        gear_curve[0].value = 0;
        gear_curve[0].time = 0;
        gear_curve[1].value = gear_direction_length; //здесь нужно будет вычислить длину сектора
        gear_curve[1].time = gear_direction_length;
        gear_tracks_position_curve.keys = gear_curve;

        for (int i = 0; i < gear_direction_rotation_point.transform.childCount; i++)
        {
            GameObject current_rot_track_obj = gear_direction_rotation_point.transform.GetChild(i).gameObject;
            current_rot_track_obj.SetActive(false);
        }


        for (int i = 0; i < gear_tracks_count; i++)
        {
            GameObject current_rot_dir_obj = gear_direction_rotation_point.transform.GetChild(i).gameObject;
            current_rot_dir_obj.SetActive(true);

            Transform current_rot_dir = current_rot_dir_obj.transform;
            current_rot_dir.localEulerAngles = new Vector3(270f + gear_direction_angled_length / gear_direction_length * -gear_tracks_position_curve.Evaluate(gear_shag_full * i + tracks_position_speed * gear_shag_full / shag), 0f, 0f);
            current_rot_dir.GetChild(0).localPosition = new Vector3(0, -m_gear_wheel_radius - track_high, 0);
            current_rot_dir.GetChild(0).localScale = new Vector3(1 * m_tracks_width, 1, 1 * (gear_shag_full / track_part_length));

        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion
        //---------------------------------------------------------------------------------------------------------------------
        #region from_gear_to_main
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //НАПРАВЛЕНИЯ ДИРЕКШЕНОВ КАСАТЕЛЬНОЙ ОТ ЗУБЧАТОГО
        from_gear_to_main_napr_direction_rotation_point_offset_proverka.transform.localPosition = new Vector3(0, -m_main_wheel_radius - track_high, 0);
        from_gear_to_main_direction_rotation_point.transform.localEulerAngles = new Vector3(90f - angle_betwen_UP_and_gear_down_to_main_down, 0, 0);
        from_gear_to_main_direction_rotation_point.transform.localPosition = new Vector3(0, 0, 0);
        from_gear_to_main_direction.transform.localPosition = new Vector3(0, -m_gear_wheel_radius - track_high, 0);
        from_gear_to_main_direction.transform.LookAt(from_gear_to_main_napr_direction_rotation_point_offset_proverka.transform,Up_direction);
        // РАБОТА С КРИВЫМИ ОТ ЗУБЧАТОГО К ОСНОВНОМУ КАТКУ
        Vector3 from_gear_to_main_tracks_direction_length_vec = from_gear_to_main_direction.transform.position - from_gear_to_main_napr_direction_rotation_point_offset_proverka.transform.position;
        float from_gear_to_main_tracks_direction_length = from_gear_to_main_tracks_direction_length_vec.magnitude;

        int from_gear_to_main_tracks_count = Mathf.Clamp(Mathf.FloorToInt(from_gear_to_main_tracks_direction_length / track_part_length), 1, 100);

        float from_gear_to_main_shag = from_gear_to_main_tracks_direction_length / (float)from_gear_to_main_tracks_count;

        from_gear_to_main_tracks_position_curve.postWrapMode = WrapMode.Loop;
        from_gear_to_main_tracks_position_curve.preWrapMode = WrapMode.Loop;
       
        Keyframe[] main_keys_of_from_gear_to_main_curve = from_gear_to_main_tracks_position_curve.keys;
        main_keys_of_from_gear_to_main_curve[0].value = 0f;
        main_keys_of_from_gear_to_main_curve[0].time = 0f;
        main_keys_of_from_gear_to_main_curve[1].value = from_gear_to_main_tracks_direction_length;
        main_keys_of_from_gear_to_main_curve[1].time = from_gear_to_main_tracks_direction_length;
        from_gear_to_main_tracks_position_curve.keys = main_keys_of_from_gear_to_main_curve;


        for (int i = 0; i < from_gear_to_main_direction.transform.childCount; i++)
        {
            GameObject current_rot_dir_obj = from_gear_to_main_direction.transform.GetChild(i).gameObject;
            current_rot_dir_obj.SetActive(false);
        }
        //
        for (int i = 0; i < from_gear_to_main_tracks_count; i++)
        {
            GameObject current_rot_dir_obj = from_gear_to_main_direction.transform.GetChild(i).gameObject;
            current_rot_dir_obj.SetActive(true);

            Transform current_rot_dir = current_rot_dir_obj.transform;
            current_rot_dir.localPosition = new Vector3(0f, 0f, from_gear_to_main_tracks_position_curve.Evaluate(from_gear_to_main_shag * i + tracks_position_speed * from_gear_to_main_shag / shag));
            current_rot_dir.localScale = new Vector3(1 * m_tracks_width, 1, 1 * (from_gear_to_main_shag / track_part_length));
        }
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion
        //---------------------------------------------------------------------------------------------------------------------
        #region last_main_angled
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        //ОТ КАСАТЕЛЬНОЙ К КАТКУ 
        //длина from_gear_to_main_napr_direction будет браться для расчета длины касательной от зубчатого к angled из предыдущего кадра(для инструкции которая идет раньше этой)
        from_gear_to_main_napr_direction_offset_point.transform.localPosition = new Vector3(0, Main_wheel_shapes[0].transform.localPosition.y, 0);
        from_gear_to_main_napr_direction_rotation_point.transform.localEulerAngles = new Vector3(90f - angle_betwen_UP_and_gear_to_main, 0, 0);
       
        from_gear_to_main_napr_direction.transform.localPosition = new Vector3(0, -m_main_wheel_radius - track_high, 0); 
        from_gear_to_main_napr_direction.transform.LookAt(track_direction[0].transform, Up_direction);
        
        //РАБОТА С КРИВЫМИ ОТ КАСАТЕЛЬНОЙ К КАТКУ
        // float l_value = gear_shag_full * track_part_length;//////
        float l_value =0f;
        Vector3 from_gear_angled_to_main_tracks_direction_length_vec = Last_main_offset_down_point.transform.position - track_direction[0].transform.position;
        float from_gear_angled_to_main_tracks_direction_length = from_gear_angled_to_main_tracks_direction_length_vec.magnitude + l_value;


        int from_gear_angled_to_main_tracks_count = Mathf.Clamp(Mathf.FloorToInt(from_gear_angled_to_main_tracks_direction_length / track_part_length), 1, 100);

        float from_gear_angled_to_main_tracks_shag = from_gear_angled_to_main_tracks_direction_length / (float)from_gear_angled_to_main_tracks_count;

        from_gear_to_main_napr_tracks_position_curve.postWrapMode = WrapMode.Loop;
        from_gear_to_main_napr_tracks_position_curve.preWrapMode = WrapMode.Loop;
        Keyframe[] from_gear_kos_to_main_tracks_position_curve = from_gear_to_main_napr_tracks_position_curve.keys;
        from_gear_kos_to_main_tracks_position_curve[0].value = -l_value;
        from_gear_kos_to_main_tracks_position_curve[0].time = -l_value;
        from_gear_kos_to_main_tracks_position_curve[1].value = from_gear_angled_to_main_tracks_direction_length;
        from_gear_kos_to_main_tracks_position_curve[1].time = from_gear_angled_to_main_tracks_direction_length;
        from_gear_to_main_napr_tracks_position_curve.keys = from_gear_kos_to_main_tracks_position_curve;

        for (int i = 0; i < from_gear_to_main_napr_direction.transform.childCount; i++)
        {
            GameObject current_rot_dir_obj = from_gear_to_main_napr_direction.transform.GetChild(i).gameObject;
            current_rot_dir_obj.SetActive(false);
        }

        for (int i = 0; i < from_gear_angled_to_main_tracks_count; i++)
        {
            GameObject current_rot_dir_obj = from_gear_to_main_napr_direction.transform.GetChild(i).gameObject;
            current_rot_dir_obj.SetActive(true);

            Transform current_rot_dir = current_rot_dir_obj.transform;
            current_rot_dir.localPosition = new Vector3(0f, 0f, from_gear_to_main_napr_tracks_position_curve.Evaluate(from_gear_angled_to_main_tracks_shag * i + tracks_position_speed * from_gear_angled_to_main_tracks_shag / shag));
            current_rot_dir.localScale = new Vector3(1 * m_tracks_width, 1, 1 * (from_gear_angled_to_main_tracks_shag / track_part_length));
        }

        //ОПРЕДЕЛЕНИЕ ЦЕЛИ КАСАТЕЛЬНЫХ ЗАДНИХ
        Transform kasateln_last_target = null;
        for (int j = 0; j < from_gear_angled_to_main_tracks_count; j++)
        {
            GameObject current_track = from_gear_to_main_napr_direction.transform.GetChild(j).gameObject;
            if (current_track.transform.localPosition.z < from_gear_angled_to_main_tracks_shag)
            {
                kasateln_last_target = current_track.transform.transform;
            }
        }//
        //СМОТРЯТ В ЦЕЛЬ
        for (int i = 0; i < from_gear_to_main_tracks_count; i++)
        {
            GameObject current_rot_dir_obj = from_gear_to_main_direction.transform.GetChild(i).gameObject;
            Transform current_rot_dir = current_rot_dir_obj.transform;
            if (current_rot_dir.localPosition.z > (from_gear_to_main_tracks_direction_length - from_gear_to_main_shag) && current_rot_dir.localPosition.z < from_gear_to_main_tracks_direction_length)
            {
                current_rot_dir.LookAt(kasateln_last_target, Up_direction);

            }
            else
            {
                current_rot_dir.localEulerAngles = new Vector3(0, 0, 0);
            }
        }////
        //ОПРЕДЕЛЕНИЕ ЦЕЛИ ОТ КАСАТЕЛЬНОЙ К ОСНОВНЫМ
        Transform angled_last_target = null;
        for (int j = 0; j < main_tracks_count; j++)
        {
            GameObject current_track = track_direction[0].transform.GetChild(j).gameObject;
            if (current_track.transform.localPosition.z < shag)
            {
                angled_last_target = current_track.transform.transform;
            }
        }
        //СМОТРЯТ В ЦЕЛЬ
        for (int i = 0; i < from_gear_angled_to_main_tracks_count; i++)
        {
            GameObject current_rot_dir_obj = from_gear_to_main_napr_direction.transform.GetChild(i).gameObject;
            Transform current_rot_dir = current_rot_dir_obj.transform;
            if (current_rot_dir.localPosition.z > (from_gear_angled_to_main_tracks_direction_length - from_gear_angled_to_main_tracks_shag))
            {
                current_rot_dir.LookAt(angled_last_target, Up_direction);

            }
            else
            {
                current_rot_dir.localEulerAngles = new Vector3(0, 0, 0);
            }
        }
        //тут
        //Если расстояние Angled Front BACK траков слишком маленькое//
        if (from_gear_angled_to_main_tracks_direction_length < minimal_track_length)
        {
            for (int i = 0; i < from_gear_to_main_napr_direction.transform.childCount; i++)
            {
                GameObject current_rot_dir_obj = from_gear_to_main_napr_direction.transform.GetChild(i).gameObject;
                current_rot_dir_obj.SetActive(false);
            }
            from_gear_to_main_direction.transform.LookAt(track_direction[0].transform, Up_direction);

            //ОПРЕДЕЛЕНИЕ ЦЕЛИ
            Transform kasateln_last_target_if_little = null;
            for (int j = 0; j < main_tracks_count; j++)
            {
                GameObject current_track = track_direction[0].transform.GetChild(j).gameObject;
                if (current_track.transform.localPosition.z < shag)
                {
                    kasateln_last_target_if_little = current_track.transform.transform;
                }
            }//



            #region from_gear_to_main_kos_again
            // РАБОТА С КРИВЫМИ ОТ ЗУБЧАТОГО К ОСНОВНОМУ КАТКУ
            from_gear_to_main_tracks_direction_length_vec = from_gear_to_main_direction.transform.position - track_direction[0].transform.position;
            from_gear_to_main_tracks_direction_length = from_gear_to_main_tracks_direction_length_vec.magnitude;

            from_gear_to_main_tracks_count = Mathf.Clamp(Mathf.FloorToInt(from_gear_to_main_tracks_direction_length / track_part_length), 1, 100);

            from_gear_to_main_shag = from_gear_to_main_tracks_direction_length / (float)from_gear_to_main_tracks_count;

            from_gear_to_main_tracks_position_curve.postWrapMode = WrapMode.Loop;
            from_gear_to_main_tracks_position_curve.preWrapMode = WrapMode.Loop;

            main_keys_of_from_gear_to_main_curve = from_gear_to_main_tracks_position_curve.keys;
            main_keys_of_from_gear_to_main_curve[0].value = 0f;
            main_keys_of_from_gear_to_main_curve[0].time = 0f;
            main_keys_of_from_gear_to_main_curve[1].value = from_gear_to_main_tracks_direction_length;
            main_keys_of_from_gear_to_main_curve[1].time = from_gear_to_main_tracks_direction_length;
            from_gear_to_main_tracks_position_curve.keys = main_keys_of_from_gear_to_main_curve;


            for (int i = 0; i < from_gear_to_main_direction.transform.childCount; i++)
            {
                GameObject current_rot_dir_obj = from_gear_to_main_direction.transform.GetChild(i).gameObject;
                current_rot_dir_obj.SetActive(false);
            }

            for (int i = 0; i < from_gear_to_main_tracks_count; i++)
            {
                GameObject current_rot_dir_obj = from_gear_to_main_direction.transform.GetChild(i).gameObject;
                current_rot_dir_obj.SetActive(true);

                Transform current_rot_dir = current_rot_dir_obj.transform;
                current_rot_dir.localPosition = new Vector3(0f, 0f, from_gear_to_main_tracks_position_curve.Evaluate(from_gear_to_main_shag * i + tracks_position_speed * from_gear_to_main_shag / shag));
                current_rot_dir.localScale = new Vector3(1 * m_tracks_width, 1, 1 * (from_gear_to_main_shag / track_part_length));
            }
            #endregion





            //СМОТРЯТ В ЦЕЛЬ
            for (int i = 0; i < from_gear_to_main_tracks_count; i++)
            {
                GameObject current_rot_dir_obj = from_gear_to_main_direction.transform.GetChild(i).gameObject;
                Transform current_rot_dir = current_rot_dir_obj.transform;
                //if (current_rot_dir.localPosition.z > (from_gear_to_main_tracks_direction_length - from_gear_to_main_shag))
                if (current_rot_dir.localPosition.z > (from_gear_to_main_tracks_direction_length - from_gear_to_main_shag) && current_rot_dir.localPosition.z < from_gear_to_main_tracks_direction_length)
                {
                    current_rot_dir.LookAt(kasateln_last_target_if_little, Up_direction);

                }
                else
                {
                    current_rot_dir.localEulerAngles = new Vector3(0, 0, 0);
                }
            }////
           
            //from_gear_to_main_direction.transform.GetChild(0).gameObject.SetActive(false);
            //ServiceProvider.Instance.GameWorld.ShowStatusMessage(from_gear_to_main_direction.transform.GetChild(0).localPosition.z.ToString(), 5f);
        }


        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion
        //---------------------------------------------------------------------------------------------------------------------



        //ServiceProvider.Instance.GameWorld.ShowStatusMessage(free_tracks_direction_angled_length.ToString() + "\n"+ free_tracks_direction_length.ToString() + "\n"+ sup_tracks_position_curve.keys[1].value.ToString() +"\n" + sup_tracks_position_curve.keys[1].time.ToString(), 5f);
        //ServiceProvider.Instance.GameWorld.ShowStatusMessage("\n\n\n\n" + gear_direction_rotation_point.transform.GetChild(0).localEulerAngles.x.ToString() + "\n"
        //    + gear_direction_rotation_point.transform.GetChild(1).localEulerAngles.x.ToString() + "\n"
        //    + gear_direction_rotation_point.transform.GetChild(2).localEulerAngles.x.ToString() + "\n"
        //    + gear_direction_rotation_point.transform.GetChild(3).localEulerAngles.x.ToString(),
        //    5f);

        //ServiceProvider.Instance.GameWorld.ShowStatusMessage("\n\n\n\n" + from_gear_to_main_direction.transform.GetChild(0).localPosition.z.ToString() + "\n"
        //    + from_gear_to_main_direction.transform.GetChild(1).localPosition.z.ToString() + "\n"
        //    + from_gear_to_main_direction.transform.GetChild(2).localPosition.z.ToString() + "\n"
        //    + from_gear_to_main_direction.transform.GetChild(3).localPosition.z.ToString(),
        //    5f);
    }

    void Delete_lishnee()
    {
        for(int i =1; i< m_wheel_count-1; i++)
        {
            DestroyImmediate(Main_wheel_mounts[i].transform.GetChild(9).gameObject);
            DestroyImmediate(Main_wheel_mounts[i].transform.GetChild(8).gameObject);
            DestroyImmediate(Main_wheel_mounts[i].transform.GetChild(7).gameObject);
            DestroyImmediate(Main_wheel_mounts[i].transform.GetChild(6).gameObject);
            DestroyImmediate(Main_wheel_mounts[i].transform.GetChild(5).gameObject);
            
        }

        DestroyImmediate(Main_wheel_mounts[0].transform.GetChild(6).gameObject);
        DestroyImmediate(Main_wheel_mounts[0].transform.GetChild(5).gameObject);

        DestroyImmediate(Main_wheel_mounts[m_wheel_count - 1].transform.GetChild(7).gameObject);
        DestroyImmediate(Main_wheel_mounts[m_wheel_count - 1].transform.GetChild(4).gameObject);


        for (int i = 0; i < m_support_wheel_count - 1; i++)
        {
            DestroyImmediate(Support_wheel_mounts[i].transform.GetChild(3).gameObject);  
        }

        DestroyImmediate(Support_wheel_mounts[m_support_wheel_count - 1].transform.GetChild(2).gameObject);
    }

    void show_something()
    {//////
        //string s_00 = "Childs: " + Main_wheel_mounts[0].transform.childCount.ToString() + "\n";
        //string s_01 = "Childs: " + Main_wheel_mounts[1].transform.childCount.ToString() + "\n";
        //string s_02 = "Childs: " + Main_wheel_mounts[2].transform.childCount.ToString() + "\n";
        //string s_03 = "Childs: " + Main_wheel_mounts[3].transform.childCount.ToString() + "\n";
        //string s_04 = "Childs: " + Main_wheel_mounts[4].transform.childCount.ToString() + "\n";
        //string s_05 = "Childs: " + Main_wheel_mounts[5].transform.childCount.ToString() + "\n";

        //string ss0 = "Childs: " + Support_wheel_mounts[0].transform.childCount.ToString() + "\n";
        //string ss1 = "Childs: " + Support_wheel_mounts[1].transform.childCount.ToString() + "\n";
        //string ss2 = "Childs: " + Support_wheel_mounts[2].transform.childCount.ToString() + "\n";
        //string ss3 = "Childs: " + Support_wheel_mounts[3].transform.childCount.ToString() + "\n";
        //string ss4 = "Childs: " + Support_wheel_mounts[4].transform.childCount.ToString() + "\n";
        //string ms0 = "Childs: " + Main_wheel_mounts[0].transform.GetChild(4).childCount.ToString() + "\n";
        //string ms1 = "Childs: " + Main_wheel_mounts[1].transform.GetChild(4).childCount.ToString() + "\n";
        //string ms2 = "Childs: " + Main_wheel_mounts[2].transform.GetChild(4).childCount.ToString() + "\n";
        //string ms3 = "Childs: " + main_tracks_mass.Length.ToString() + "\n";
        //string sd0 = sup_direction[0].transform.childCount.ToString() + "\n";
        //string sd1 = sup_direction[1].transform.childCount.ToString() + "\n";
        //string sd2 = sup_direction[2].transform.childCount.ToString() + "\n";
       // string sd3 = sup_direction[3].transform.childCount.ToString() + "\n";
        ////string f0 = free_direction_rotation_point.transform.childCount.ToString() + "\n";
        //string fmtf = from_main_to_free_direction.transform.childCount.ToString() + "\n";
        //string md = main_direction_front_rotation_point.transform.GetChild(0).childCount.ToString() + "\n";//
        //string ffts = from_free_to_sup_direction.transform.childCount.ToString() + "\n";
        //string gt = gear_direction_rotation_point.transform.childCount.ToString() + "\n";
        //string tgear = from_gear_to_main_direction.transform.childCount.ToString() + "\n";
        //string lm = from_gear_to_main_napr_direction_rotation_point.transform.childCount.ToString() + "\n";
        //string fstg = from_sup_to_gear_direction.transform.childCount.ToString() + "\n";
        ////string s_0 = Main_wheel_mounts[0].transform.GetChild(0).name.ToString() + "\n";
        ////string s_1 = Main_wheel_mounts[0].transform.GetChild(1).name.ToString() + "\n";
        ////string s_2 = Main_wheel_mounts[0].transform.GetChild(2).name.ToString() + "\n";
        ////string s_3 = Main_wheel_mounts[0].transform.GetChild(3).name.ToString() + "\n";
        ////string s_4 = Main_wheel_mounts[0].transform.GetChild(4).name.ToString() + "\n";
        ////string s_5 = Main_wheel_mounts[0].transform.GetChild(5).name.ToString() + "\n";
        ////string s_6 = Main_wheel_mounts[0].transform.GetChild(6).name.ToString() + "\n";
        ////string s_7 = Main_wheel_mounts[0].transform.GetChild(7).name.ToString() + "\n";
        ////string s_8 = Main_wheel_mounts[0].transform.GetChild(8).name.ToString() + "\n";
        ////string s_9 = Main_wheel_mounts[0].transform.GetChild(9).name.ToString() + "\n";
        ////ServiceProvider.Instance.GameWorld.ShowStatusMessage("\n\n\n\n\n\n\n" + s_00 + s_01 + s_02 + s_03+ s_04+ s_05, 5f);
        ////ServiceProvider.Instance.GameWorld.ShowStatusMessage("\n\n\n\n\n\n\n" + s_00 + s_0 + s_1 + s_2 + s_3 + s_4 + s_5 + s_6 + s_7 + s_8 + s_9, 5f);
        ////ServiceProvider.Instance.GameWorld.ShowStatusMessage("\n\n\n\n\n\n\n" + ss0 + ss1 + ss2 + ss3 + ss4, 5f);
        //ServiceProvider.Instance.GameWorld.ShowStatusMessage("\n\n\n\n\n\n\n" + ms0 + ms1 + ms2 + ms3, 5f);
        //ServiceProvider.Instance.GameWorld.ShowStatusMessage("\n\n\n\n\n\n\n" + sd0 + sd1 + sd2 + sd3, 5f);//
        //ServiceProvider.Instance.GameWorld.ShowStatusMessage("\n\n\n\n\n\n\n" + (free_direction_rotation_point.transform.GetChild(0).GetChild(0).GetChild(2).localPosition.z).ToString() + "\n"
        //                                                                      + (free_direction_rotation_point.transform.GetChild(1).GetChild(0).GetChild(2).localPosition.z).ToString() + "\n"
        //                                                                      + (free_direction_rotation_point.transform.GetChild(2).GetChild(0).GetChild(2).localPosition.z).ToString() + "\n"
        //                                                                      + (free_direction_rotation_point.transform.GetChild(3).GetChild(0).GetChild(2).localPosition.z).ToString(), 5f);
    }

    void create_tracks_part_copyes()
    {
        
       
        for(int i =0; i < main_tracks_count-1; i++ )
        {
            GameObject Copy_track = (GameObject)Instantiate(Main_tracks_part_first_track, Main_tracks_part_first_track.transform.position, Main_tracks_part_first_track.transform.rotation);
            Copy_track.transform.parent = Main_tracks_part_papa.transform;
            Copy_track.transform.localPosition = new Vector3(Main_tracks_part_first_track.transform.localPosition.x, Main_tracks_part_first_track.transform.localPosition.y, Main_tracks_part_first_track.transform.localPosition.z);
            Copy_track.transform.localRotation = Main_tracks_part_first_track.transform.localRotation;
            Copy_track.transform.localScale = Main_tracks_part_first_track.transform.localScale;
        }

        for (int i = 0; i < sup_tracks_count - 1; i++)
        {
            GameObject Copy_track = (GameObject)Instantiate(Sup_tracks_part_first_track, Sup_tracks_part_first_track.transform.position, Sup_tracks_part_first_track.transform.rotation);
            Copy_track.transform.parent = Sup_tracks_part_papa.transform;
            Copy_track.transform.localPosition = new Vector3(Sup_tracks_part_first_track.transform.localPosition.x, Sup_tracks_part_first_track.transform.localPosition.y, Sup_tracks_part_first_track.transform.localPosition.z);
            Copy_track.transform.localRotation = Sup_tracks_part_first_track.transform.localRotation;
            Copy_track.transform.localScale = Sup_tracks_part_first_track.transform.localScale;
        }

        float rotation_length = Mathf.PI * (m_free_wheel_radius + track_high) * 180f / 180f;
        float rotating_tracks_count = Mathf.Clamp(Mathf.FloorToInt(rotation_length / track_part_length) + 1,1,100);
        for (int i = 0; i < rotating_tracks_count - 1; i++)
        {
            GameObject Copy_track = (GameObject)Instantiate(Free_tracks_part_first_track, Free_tracks_part_first_track.transform.position, Free_tracks_part_first_track.transform.rotation);
            Copy_track.transform.parent = Free_tracks_part_papa.transform;
            Copy_track.transform.localPosition = new Vector3(Free_tracks_part_first_track.transform.localPosition.x, Free_tracks_part_first_track.transform.localPosition.y, Free_tracks_part_first_track.transform.localPosition.z);
            Copy_track.transform.localRotation = Free_tracks_part_first_track.transform.localRotation;
            Copy_track.transform.localScale = Free_tracks_part_first_track.transform.localScale;
        }


        float add_some_dist = track_part_length / 1f;
        float val1 = m_Tracks_length - (m_Main_wheel_spacing * (m_wheel_count - 1) + m_Gear_to_first_main_length) + m_lever_length;
        float val2 = m_Main_wheel_mount_offset + m_lever_length;
        float val3 = Mathf.Sqrt(val1 * val1 + val2 * val2);
        float from_main_to_free = m_Tracks_length - (m_Main_wheel_spacing * (m_wheel_count - 1) + m_Gear_to_first_main_length) + m_lever_length;
        float from_main_to_free_tracks_direction_length = (val3 + add_some_dist);
        int from_main_to_free_tracks_count = Mathf.Clamp(Mathf.FloorToInt(from_main_to_free_tracks_direction_length / track_part_length),1,100);
        for (int i = 0; i < from_main_to_free_tracks_count - 1; i++)
        {
            GameObject Copy_track = (GameObject)Instantiate(from_main_to_free_tracks_part_first_track, from_main_to_free_tracks_part_first_track.transform.position, from_main_to_free_tracks_part_first_track.transform.rotation);
            Copy_track.transform.parent = from_main_to_free_tracks_part_papa.transform;
            Copy_track.transform.localPosition = new Vector3(from_main_to_free_tracks_part_first_track.transform.localPosition.x, from_main_to_free_tracks_part_first_track.transform.localPosition.y, from_main_to_free_tracks_part_first_track.transform.localPosition.z);
            Copy_track.transform.localRotation = from_main_to_free_tracks_part_first_track.transform.localRotation;
            Copy_track.transform.localScale = from_main_to_free_tracks_part_first_track.transform.localScale;
        }

        float front_angled_length = 2 * (m_main_wheel_radius + track_high) * Mathf.Sin(90f * Mathf.Deg2Rad / 2f);
        float front_angled_count = Mathf.Clamp(Mathf.FloorToInt(front_angled_length / track_part_length) + 1, 1, 100);
        for (int i = 0; i < front_angled_count; i++)//
        {
            GameObject Copy_track = (GameObject)Instantiate(front_main_angled_tracks_part_first_track, front_main_angled_tracks_part_first_track.transform.position, front_main_angled_tracks_part_first_track.transform.rotation);
            Copy_track.transform.parent = front_main_angled_tracks_part_papa.transform;
            Copy_track.transform.localPosition = new Vector3(front_main_angled_tracks_part_first_track.transform.localPosition.x, front_main_angled_tracks_part_first_track.transform.localPosition.y, front_main_angled_tracks_part_first_track.transform.localPosition.z);
            Copy_track.transform.localRotation = front_main_angled_tracks_part_first_track.transform.localRotation;
            Copy_track.transform.localScale = front_main_angled_tracks_part_first_track.transform.localScale;
        }



        float val11 = Mathf.Abs(m_Free_to_first_support_length);
        float val22 = Mathf.Abs(m_Support_wheel_mount_offset);
        float val33 = Mathf.Sqrt(val11 * val11 + val22 * val22);
        float from_free_to_sup_tracks_direction_length = val33;
        int from_free_to_sup_tracks_count = Mathf.Clamp(Mathf.FloorToInt(from_free_to_sup_tracks_direction_length / track_part_length) + 1, 1, 100);
        for (int i = 0; i < from_free_to_sup_tracks_count - 1; i++)
        {
            GameObject Copy_track = (GameObject)Instantiate(from_free_to_sup_tracks_part_first_track, from_free_to_sup_tracks_part_first_track.transform.position, from_free_to_sup_tracks_part_first_track.transform.rotation);
            Copy_track.transform.parent = from_free_to_sup_tracks_part_papa.transform;
            Copy_track.transform.localPosition = new Vector3(from_free_to_sup_tracks_part_first_track.transform.localPosition.x, from_free_to_sup_tracks_part_first_track.transform.localPosition.y, from_free_to_sup_tracks_part_first_track.transform.localPosition.z);
            Copy_track.transform.localRotation = from_free_to_sup_tracks_part_first_track.transform.localRotation;
            Copy_track.transform.localScale = from_free_to_sup_tracks_part_first_track.transform.localScale;
        }


        float gear_rotation_length = Mathf.PI * (m_gear_wheel_radius + track_high) * 180f / 180f;
        float gear_rotating_tracks_count = Mathf.Clamp(Mathf.FloorToInt(gear_rotation_length / track_part_length) + 1, 1, 100);
        for (int i = 0; i < gear_rotating_tracks_count - 1; i++)
        {
            GameObject Copy_track = (GameObject)Instantiate(Gear_tracks_part_first_track, Gear_tracks_part_first_track.transform.position, Gear_tracks_part_first_track.transform.rotation);
            Copy_track.transform.parent = Gear_tracks_part_papa.transform;
            Copy_track.transform.localPosition = new Vector3(Gear_tracks_part_first_track.transform.localPosition.x, Gear_tracks_part_first_track.transform.localPosition.y, Gear_tracks_part_first_track.transform.localPosition.z);
            Copy_track.transform.localRotation = Gear_tracks_part_first_track.transform.localRotation;
            Copy_track.transform.localScale = Gear_tracks_part_first_track.transform.localScale;
        }




        float gval1 = m_Gear_to_first_main_length;
        float gval2 = m_Main_wheel_mount_offset + m_lever_length;
        float gval3 = Mathf.Sqrt(gval1 * gval1 + gval2 * gval2);
     //nen
        float from_gear_to_main_tracks_direction_length = gval3;
        int from_gear_to_main_tracks_count = Mathf.Clamp(Mathf.FloorToInt(from_gear_to_main_tracks_direction_length / track_part_length) + 1, 1, 100);
        for (int i = 0; i < from_gear_to_main_tracks_count; i++)
        {
            GameObject Copy_track = (GameObject)Instantiate(from_gear_to_main_tracks_part_first_track, from_gear_to_main_tracks_part_first_track.transform.position, from_gear_to_main_tracks_part_first_track.transform.rotation);
            Copy_track.transform.parent = from_gear_to_main_tracks_part_papa.transform;
            Copy_track.transform.localPosition = new Vector3(from_gear_to_main_tracks_part_first_track.transform.localPosition.x, from_gear_to_main_tracks_part_first_track.transform.localPosition.y, from_gear_to_main_tracks_part_first_track.transform.localPosition.z);
            Copy_track.transform.localRotation = from_gear_to_main_tracks_part_first_track.transform.localRotation;
            Copy_track.transform.localScale = from_gear_to_main_tracks_part_first_track.transform.localScale;
        }

        //
        float gear_to_main_angled_rotation_length = 2 * (m_main_wheel_radius + track_high) * Mathf.Sin(90f * Mathf.Deg2Rad / 2f);
        float gear_to_main_rotating_tracks_count = Mathf.Clamp(Mathf.FloorToInt(gear_to_main_angled_rotation_length / track_part_length) + 1, 1, 100);
        for (int i = 0; i < gear_to_main_rotating_tracks_count; i++)
        {
            GameObject Copy_track = (GameObject)Instantiate(Gear_to_main_tracks_part_first_track, Gear_to_main_tracks_part_first_track.transform.position, Gear_to_main_tracks_part_first_track.transform.rotation);
            Copy_track.transform.parent = Gear_to_main_tracks_part_papa.transform;
            Copy_track.transform.localPosition = new Vector3(Gear_to_main_tracks_part_first_track.transform.localPosition.x, Gear_to_main_tracks_part_first_track.transform.localPosition.y, Gear_to_main_tracks_part_first_track.transform.localPosition.z);
            Copy_track.transform.localRotation = Gear_to_main_tracks_part_first_track.transform.localRotation;
            Copy_track.transform.localScale = Gear_to_main_tracks_part_first_track.transform.localScale;
        }





        float sgval1 = m_Tracks_length - (m_Support_wheel_spacing * (m_support_wheel_count - 1) + m_Free_to_first_support_length);
        float sgval2 = Mathf.Abs(m_Support_wheel_mount_offset);
        float sgval3 = Mathf.Sqrt(sgval1 * sgval1 + sgval2 * sgval2);
        float from_sup_to_gear_tracks_direction_length = sgval3;
        int from_sup_to_gear_tracks_count = Mathf.Clamp(Mathf.FloorToInt(from_sup_to_gear_tracks_direction_length / track_part_length) + 1, 1, 100);
        for (int i = 0; i < from_sup_to_gear_tracks_count - 1; i++)
        {
            GameObject Copy_track = (GameObject)Instantiate(from_sup_to_gear_tracks_part_first_track, from_sup_to_gear_tracks_part_first_track.transform.position, from_sup_to_gear_tracks_part_first_track.transform.rotation);
            Copy_track.transform.parent = from_sup_to_gear_tracks_part_papa.transform;
            Copy_track.transform.localPosition = new Vector3(from_sup_to_gear_tracks_part_first_track.transform.localPosition.x, from_sup_to_gear_tracks_part_first_track.transform.localPosition.y, from_sup_to_gear_tracks_part_first_track.transform.localPosition.z);
            Copy_track.transform.localRotation = from_sup_to_gear_tracks_part_first_track.transform.localRotation;
            Copy_track.transform.localScale = from_sup_to_gear_tracks_part_first_track.transform.localScale;
        }


    }


    void Find_tracks()
    {
       // Tracks_in_children.add

        for(int i=0;i<500;i++)
        {
            
            Transform Current_track = transform.Find("Game_track_t_80_low_poly");
            if(Current_track!=null)
            {
                Tracks_in_children[i] = Current_track;
            }
            
        }

        foreach(var track in Tracks_in_children)
        {
            track.transform.localPosition = new Vector3(track.transform.localPosition.x, m_tracks_thickness*10f, track.transform.localPosition.z);
        }
        
    }

    void Designer_timer()
	{
		var modifer = (Tracks_2)PartModifier;
		mdi++;

		if (mdi > 99997) 
		{
			mdi = 1;
		}
		Side ();////
        float mm_invisible_support_wheels = 0f;
        if (modifer.is_support_wheels_visible == "Yes")
        {
            mm_invisible_support_wheels = 1;
        }

        if (modifer.is_support_wheels_visible == "No")
        {
            mm_invisible_support_wheels = 0;
        }
        //
        mass_designer [mdi] = modifer.wheel_count + modifer.main_wheel_radius + modifer.gear_wheel_radius + modifer.free_wheel_radius + modifer.Gear_to_first_main_length + modifer.Tracks_length + modifer.Main_wheel_spacing + modifer.Main_wheel_mount_offset  + side + modifer.support_wheel_count + modifer.Free_to_first_support_length + modifer.Support_wheel_mount_offset + modifer.Support_wheel_spacing+modifer.support_wheel_radius + modifer.suspension_lever_length + modifer.suspension_lever_offset + mm_invisible_support_wheels + modifer.main_wheel_width + modifer.free_wheel_width + modifer.support_wheel_width + modifer.gear_wheel_width+modifer.Free_wheel_horizontal_offset+modifer.Gear_wheel_horizontal_offset+modifer.Support_wheel_horizontal_offset + modifer.x_offset_of_1_3_5 + modifer.x_offset_of_2_4_6 + modifer.Gear_wheel_mount_offset;

        //modifer.tracks_thickness//
        if (mass_designer [mdi]==mass_designer [mdi-1])
		{

			answer= false;
		}else 
			answer=true;
	}



//	void OnGUI()
//	{


//		string[] many2 = 
		
//		{
//			//Main_wheel_mounts [0].transform.localPosition.ToString() + "\n"
////			zzzz[1].ToString() + "\n"+
////			zzzz[2].ToString() + "\n"

//			//sum_hits.ToString() + "\n"+
////			vertices_up_side.Length.ToString() + "\n"+
////			triangles_up_side.Length.ToString() + "\n"
////			forward_force[2].ToString() + "\n"+
////			forward_force[3].ToString() + "\n"+
////			forward_force[4].ToString() + "\n"+
////			forward_force[5].ToString() + "\n"
//			//suspension_wheel_force[0].ToString() + "\n"
//			//+ suspension_wheel_force[1].ToString() + "\n"
//			//+ suspension_wheel_force[2].ToString() + "\n"
//			//+ suspension_wheel_force[3].ToString() + "\n"
//			//"linear_speed_x= ",this.linear_speed_x.ToString(),"\n",
//			//"linear_speed_z= ",this.linear_speed_z.ToString(),"\n",
//			//"angular_speed_y= ",this.angular_speed_y.ToString(),"\n",
//		};
//		string two = string.Concat (many2);
//		GUI.Label (new Rect (Screen.width/2, Screen.height/2, 400, 400), two.ToString ());

//	}

}

