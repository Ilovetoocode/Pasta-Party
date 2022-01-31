using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class SpriteSkinRPC : MonoBehaviour {
	[Tooltip("The new spritesheet texture to use.")]
	public Texture2D newSprite;
	[Tooltip("The path to the new spritesheet. Note that the sheets must be in the Resources folder.")]
	public string folderPath = "Retro Pixel Characters/Spritesheets/";
	[Tooltip("Set to true if this is a child sprite, like an outfit, eye color, etc.")]
	public bool isChildSprite = false;

	private Sprite[] newSpritesheet; //Spritesheet array. Populated with all sprites within a texture sheet.
	private SpriteRenderer spriteRenderer; //The object's sprite renderer.
	private SpriteRenderer parentRenderer; //The parent object's sprite renderer, used if child sprite.
	private string currentFrameName; //Name of the current frame. Used to find the current frame's number.
	private string parentFrameName; //Name of the parent's current frame. Used to find parent sprite's frame number.
	private int frameIndex = 0; //The index of the current frame. Used to set index of new frame.

	//During start, find the SpriteRenderer and load all spritesheet frames into the array.
	//Again, spritesheets need to be in the Resources folder for the Resources.LoadAll to work.
	void Start () {
		if (spriteRenderer == null)
		{
			spriteRenderer = GetComponentInParent<SpriteRenderer>(); //Set spriteRenderer to current sprite's renderer.
			if (isChildSprite == true)
			{
				parentRenderer = transform.parent.GetComponent<SpriteRenderer>(); //Set parentRenderer to parent sprite's renderer.
			}
		}
		if (newSprite != null)
		{
			newSpritesheet = Resources.LoadAll<Sprite>(folderPath + newSprite.name);
		}
	}

	//Using LateUpdate, since trying to replace sprites in Update will just be overridden by the animation.
	void LateUpdate ()
	{
		if (isChildSprite == true && parentRenderer == null)
		{
			Debug.LogError("Couldn't find SpriteRenderer in parent. Either make object child of character base or untick Is Child Sprite.");
			this.enabled = false;
			return;
		}

		//If we've got our spritesheet, our renderer and the spritesheet is actually a sheet, let's fire up the magic!
		if (newSprite != null && spriteRenderer != null && newSpritesheet.Length > 0)
		{
			//Get the currently rendered sprite's name.
			currentFrameName = spriteRenderer.sprite.name;
			//Get the parent sprite's name if child sprite.
			if (isChildSprite == true)
			{
				parentFrameName = parentRenderer.sprite.name;
			}
			//Parse out the frame number from the frame name and use as index for the new frame to render.
			//Get the parent object sprite's frame name to parse if it's a child sprite.
			if (isChildSprite == false) 
			{
				int.TryParse(Regex.Replace(currentFrameName, "[^0-9]", ""), out frameIndex);
			}
			else
			{
				int.TryParse(Regex.Replace(parentFrameName, "[^0-9]", ""), out frameIndex);
			}
			//Finally, set the new sprite to render.
			spriteRenderer.sprite = newSpritesheet[frameIndex];
		}
		else if (newSprite == null)
		{
			//If the New Sprite has not been set, log a warning and then disable this script (to prevent a new warning every frame.)
			Debug.LogWarning("New Sprite has not been set. Drag and drop your spritesheet texture to the New Sprite field.");
			this.enabled = false;
		}
		else if (spriteRenderer == null)
		{
			//If the object contains no Sprite Renderer, log an error and disable script.
			Debug.LogError("Parent does not contain a Sprite Renderer.");
			this.enabled = false;
		}
		else if (newSpritesheet.Length <= 0)
		{
			//If the new sprite sheet fails loading any sprites, it might not be a proper spritesheet. Could also be that the sprite wasn't found in the folder path.
			Debug.LogWarning("It seems there were too few sprites in the New Sprite. Was it all a ruse?! Actually, it might be the wrong Folder Path, too.");
			this.enabled = false;
		}
	}
}
