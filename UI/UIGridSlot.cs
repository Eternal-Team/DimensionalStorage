using BaseLibrary;
using BaseLibrary.UI.Elements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Terraria;
using Terraria.GameContent.Achievements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.UI.Chat;

namespace DimensionalStorage.UI
{
	public class UIGridSlot : BaseElement
	{
		public Texture2D backgroundTexture = Main.inventoryBackTexture;

		public event Action OnInteract;

		private Network.Network Network;
		public Item Item;
		private int Stack => Item.maxStack > 1 ? Network.Items.Where(item => item.type == Item.type).Sum(item => item.stack) : 1;

		public UIGridSlot(Network.Network network, Item item)
		{
			Width = Height = (40, 0);

			Network = network;
			Item = item;
		}

		/*
			public override void ScrollWheel(UIScrollWheelEvent evt)
		   {
		   if (!Main.keyState.IsKeyDown(Keys.LeftAlt)) return;
		   
		   if (evt.ScrollWheelValue > 0)
		   {
		   if (Main.mouseItem.type == Item.type && Main.mouseItem.stack < Main.mouseItem.maxStack)
		   {
		   Main.mouseItem.stack++;
		   if (--Item.stack <= 0) Item.TurnToAir();
		   }
		   else if (Main.mouseItem.IsAir)
		   {
		   Main.mouseItem = Item.Clone();
		   Main.mouseItem.stack = 1;
		   if (--Item.stack <= 0) Item.TurnToAir();
		   }
		   }
		   else if (evt.ScrollWheelValue < 0)
		   {
		   if (Item.type == Main.mouseItem.type && Item.stack < Item.maxStack)
		   {
		   Item.stack++;
		   if (--Main.mouseItem.stack <= 0) Main.mouseItem.TurnToAir();
		   }
		   else if (Item.IsAir)
		   {
		   Item = Main.mouseItem.Clone();
		   Item.stack = 1;
		   if (--Main.mouseItem.stack <= 0) Main.mouseItem.TurnToAir();
		   }
		   }
		   }
		*/

		public override void Click(UIMouseEvent evt)
		{
			Item.newAndShiny = false;
			Player player = Main.LocalPlayer;

			if (ItemSlot.ShiftInUse)
			{
				Item = Utility.PutItemInInventory(Item);

				OnInteract?.Invoke();

				base.Click(evt);

				return;
			}

			if (Main.mouseItem.IsAir) Main.mouseItem = Network.ExtractItem(Item);
			else
			{
				Network.InsertItem(ref Main.mouseItem);
				Main.PlaySound(SoundID.Grab);
			}

			if (Item.stack > 0) AchievementsHelper.NotifyItemPickup(player, Item);

			if (Main.mouseItem.type > 0 || Item.type > 0)
			{
				Recipe.FindRecipes();
				Main.PlaySound(SoundID.Grab);
			}

			OnInteract?.Invoke();

			base.Click(evt);
		}

		public override void RightClickContinuous(UIMouseEvent evt)
		{
			OnInteract?.Invoke();

			Player player = Main.LocalPlayer;
			Item.newAndShiny = false;

			if (player.itemAnimation > 0) return;

			//bool specialClick = false;
			//if (ItemLoader.CanRightClick(Item) && Main.mouseRightRelease)
			//{
			//	specialClick = true;
			//}

			//if (specialClick && Main.mouseRightRelease)
			//{
			//	OnInteract?.Invoke();
			//	return;
			//}

			if (Main.stackSplit <= 1 && Main.mouseRight)
			{
				if ((Main.mouseItem.IsTheSameAs(Item) || Main.mouseItem.type == 0) && (Main.mouseItem.stack < Main.mouseItem.maxStack || Main.mouseItem.type == 0))
				{
					if (Main.mouseItem.type == 0)
					{
						Main.mouseItem = Item.Clone();
						Main.mouseItem.stack = 0;
						if (Item.favorited && Item.maxStack == 1) Main.mouseItem.favorited = true;
						Main.mouseItem.favorited = false;
					}

					Main.mouseItem.stack++;
					Item.stack--;
					if (Item.stack <= 0) Item = new Item();

					Recipe.FindRecipes();

					Main.soundInstanceMenuTick.Stop();
					Main.soundInstanceMenuTick = Main.soundMenuTick.CreateInstance();
					Main.PlaySound(12);

					Main.stackSplit = Main.stackSplit == 0 ? 15 : Main.stackDelay;
				}
			}

			OnInteract?.Invoke();
		}

		public override int CompareTo(object obj) => -(Item.type - ((UIGridSlot)obj).Item.type);

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			spriteBatch.DrawSlot(Dimensions, Color.White, backgroundTexture);

			float scale = Math.Min(Dimensions.Width / backgroundTexture.Width, Dimensions.Height / backgroundTexture.Height);

			if (!Item.IsAir)
			{
				Texture2D itemTexture = Main.itemTexture[Item.type];
				Rectangle rect = Main.itemAnimations[Item.type] != null ? Main.itemAnimations[Item.type].GetFrame(itemTexture) : itemTexture.Frame();
				Color newColor = Color.White;
				float pulseScale = 1f;
				ItemSlot.GetItemLight(ref newColor, ref pulseScale, Item);
				int height = rect.Height;
				int width = rect.Width;
				float drawScale = 1f;

				float availableWidth = InnerDimensions.Width;
				if (width > availableWidth || height > availableWidth)
				{
					if (width > height) drawScale = availableWidth / width;
					else drawScale = availableWidth / height;
				}

				drawScale *= scale;
				Vector2 vector = backgroundTexture.Size() * scale;
				Vector2 position2 = Dimensions.Position() + vector / 2f - rect.Size() * drawScale / 2f;
				Vector2 origin = rect.Size() * (pulseScale / 2f - 0.5f);

				if (ItemLoader.PreDrawInInventory(Item, spriteBatch, position2, rect, Item.GetAlpha(newColor), Item.GetColor(Color.White), origin, drawScale * pulseScale))
				{
					spriteBatch.Draw(itemTexture, position2, rect, Item.GetAlpha(newColor), 0f, origin, drawScale * pulseScale, SpriteEffects.None, 0f);
					if (Item.color != Color.Transparent) spriteBatch.Draw(itemTexture, position2, rect, Item.GetColor(Color.White), 0f, origin, drawScale * pulseScale, SpriteEffects.None, 0f);
				}

				ItemLoader.PostDrawInInventory(Item, spriteBatch, position2, rect, Item.GetAlpha(newColor), Item.GetColor(Color.White), origin, drawScale * pulseScale);
				if (ItemID.Sets.TrapSigned[Item.type]) spriteBatch.Draw(Main.wireTexture, Dimensions.Position() + new Vector2(40f, 40f) * scale, new Rectangle(4, 58, 8, 8), Color.White, 0f, new Vector2(4f), 1f, SpriteEffects.None, 0f);
				if (Stack > 1)
					ChatManager.DrawColorCodedStringWithShadow(spriteBatch, Main.fontItemStack, Stack < 1000 ? Stack.ToString() : Stack.ToSI("N0"), Dimensions.Position() + new Vector2(10f, 26f) * scale, Color.White, 0f, Vector2.Zero, new Vector2(scale), -1f, scale);

				if (IsMouseHovering)
				{
					Main.LocalPlayer.showItemIcon = false;
					Main.ItemIconCacheUpdate(0);
					Main.HoverItem = Item.Clone();
					Main.HoverItem.stack = Stack;
					Main.hoverItemName = Main.HoverItem.Name;

					if (ItemSlot.ShiftInUse) Hooking.SetCursor("Terraria/UI/Cursor_7");
				}
			}
		}
	}
}