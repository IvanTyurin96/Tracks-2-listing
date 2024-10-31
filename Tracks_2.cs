using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Jundroo.SimplePlanes.ModTools.Parts;
using Jundroo.SimplePlanes.ModTools.Parts.Attributes;

/// <summary>
/// A part modifier for SimplePlanes.
/// A part modifier is responsible for attaching a part modifier behaviour script to a game object within a part's hierarchy.
/// </summary>
[Serializable]
public class Tracks_2 : Jundroo.SimplePlanes.ModTools.Parts.PartModifier
{
    [DesignerPropertyToggleButton("Right", "Left", Label = "Side")]
    public string Side;

    [DesignerPropertySlider(100f, 4000f, 40, Label = "Power, HP")]
    public float power;

    [DesignerPropertySlider(1f, 10f, 37, Label = "Tracks length")]
    public float Tracks_length;

    [DesignerPropertySlider(1f, 10f, 37, Label = "Tracks width")]
    public float Tracks_width;

    [DesignerPropertySlider(0.1f,2f,39,Label="Main wheel radius",Header = "Wheels radius")]
	public float main_wheel_radius;

    [DesignerPropertySlider(0.1f, 2f, 39, Label = "Gear wheel radius")]
    public float gear_wheel_radius;

    [DesignerPropertySlider(0.1f, 2f, 39, Label = "Free wheel radius")]
    public float free_wheel_radius;

    [DesignerPropertySlider(0.1f, 2f, 39, Label = "Supp. wheel radius")]
    public float support_wheel_radius;

    [DesignerPropertySlider(0.1f, 2f, 39, Label = "Main wheel width", Header = "Wheels width")]
    public float main_wheel_width;

    [DesignerPropertySlider(0.1f, 2f, 39, Label = "Free wheel width")]
    public float free_wheel_width;

    [DesignerPropertySlider(0.1f, 2f, 39, Label = "Gear wheel width")]
    public float gear_wheel_width;

    [DesignerPropertySlider(0.1f, 2f, 39, Label = "Supp. wheel width")]
    public float support_wheel_width;

    [DesignerPropertySlider(0.1f, 2f, 39, Label = "Gear-1st main length", Header = "Free/Gear to 1st")]
    public float Gear_to_first_main_length;

    [DesignerPropertySlider(0.1f, 2f, 39, Label = "Free-1st supp. length")]
    public float Free_to_first_support_length;

    [DesignerPropertySlider(0.1f, 2f, 39, Label = "Main wheel spacing", Header = "Wheels spacing")]
    public float Main_wheel_spacing;

    [DesignerPropertySlider(0.1f, 2f, 39, Label = "Support wheel spacing")]
    public float Support_wheel_spacing;


    [DesignerPropertySlider(0f, 1f, 21, Label = "Main wheel mount offset", Header = "Wheels vertical offset")]
    public float Main_wheel_mount_offset;

    [DesignerPropertySlider(-1f, 1f, 81, Label = "Supp. wheel mount offset")]
    public float Support_wheel_mount_offset;

    [DesignerPropertySlider(-1f, 1f, 81, Label = "Gear. wheel mount offset")]
    public float Gear_wheel_mount_offset;

    [DesignerPropertySlider(-1f, 1f, 41, Label = "Free wheel X offset", Header = "Wheels horizontal offset")]
    public float Free_wheel_horizontal_offset;

    [DesignerPropertySlider(-1f, 1f, 41, Label = "Gear wheel X offset")]
    public float Gear_wheel_horizontal_offset;

    [DesignerPropertySlider(-1f, 1f, 41, Label = "Supp. wheel X offset")]
    public float Support_wheel_horizontal_offset;

    [DesignerPropertySlider(-1f, 1f, 41, Label = "1,3,5... X offset", Header = "Chess offset")]
    public float x_offset_of_1_3_5;
    [DesignerPropertySlider(-1f, 1f, 41, Label = "2,4,6... X offset")]
    public float x_offset_of_2_4_6;

    [DesignerPropertySlider(2, 12, 11, Label = "Wheel count", Header = "Wheels count")]
    public int wheel_count;

	[DesignerPropertySlider(2,12,11,Label="Support wheel count")]
	public int support_wheel_count;


	//[DesignerPropertySlider(0.005f,0.05f,10,Label="Tracks thickness")]
	//public float tracks_thickness;

	[DesignerPropertySlider(1000,20000,20,Label="Spring per wheel, kgF",Header ="Suspension")]
	public float suspension_spring;

    [DesignerPropertySlider(1000, 20000, 20, Label = "Wheel damper, kgF")]
    public float suspension_damper;

    [DesignerPropertySlider(0,2f,41,Label="Lever_length, m")]
	public float suspension_lever_length;

    [DesignerPropertySlider(0, 2f, 41, Label = "Lever_offset, m")]
    public float suspension_lever_offset;

    //[DesignerPropertySlider(0,2f,41,Label="Suspension distance, m")]
	//public float suspension_distance;

	

	[DesignerPropertySlider(1000,40000,40,Label="Side friction per wheel,",Header ="Friction")]
	public float wheel_friction_side;

	[DesignerPropertySlider(1000,40000,40,Label= "Forward fric. per wheel")]
	public float wheel_friction_forward;

    [DesignerPropertySlider(100, 4000, 40, Label = "Brake force")]
    public float brake_force;

    [DesignerPropertyToggleButton("No", "Yes", Label = "Exist", Header = "Hydraulic suspension")]
    public string is_hydraulic_suspension_exist;

    [DesignerPropertySlider(0, 100, 101, Label = "Hydraulic speed")]
    public float hydraulic_speed;

    [DesignerPropertySlider(0, 2, 21, Label = "Hydraulic pitch force")]
    public float hydraulic_pitch_force;

    [DesignerPropertyToggleButton("No", "Yes", Label = "Visible supp. wheels")]
    public string is_support_wheels_visible;

    [DesignerPropertySlider(0f, 120f, 121, Label = "Max.speed forward, km/h", Header = "Speed limits")]
    public float maximal_speed_forward;
    [DesignerPropertySlider(-120f, 0f, 121, Label = "Max.speed back, km/h")]
    public float maximal_speed_back;
    [DesignerPropertySlider(2f, 4f, 21, Label = "Antispeed multiplier forward")]
    public float antispeed_multiplier_forward;
    [DesignerPropertySlider(2f, 4f, 21, Label = "Antispeed multiplier back")]
    public float antispeed_multiplier_back;
    /// <summaryanti
    /// Called when this part modifiers is being initialized as the part game object is being created.
    /// </summary>
    /// <param name="partRootObject">The root game object that has been created for the part.</param>
    /// <returns>The created part modifier behaviour, or <c>null</c> if it was not created.</returns>
    public override Jundroo.SimplePlanes.ModTools.Parts.PartModifierBehaviour Initialize(UnityEngine.GameObject partRootObject)
    {
        // Attach the behaviour to the part's root object.
        var behaviour = partRootObject.GetComponent<Tracks_2Behaviour>();
        return behaviour;
    }
}