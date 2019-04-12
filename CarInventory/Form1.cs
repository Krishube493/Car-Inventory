using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace CarInventory
{
    public partial class Form1 : Form
    {
        List<Car> inventory = new List<Car>();

        public Form1()
        {
            InitializeComponent();
            loadDB();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            string year, make, colour, mileage;

            year = yearInput.Text;
            make = makeInput.Text;
            colour = colourInput.Text;
            mileage = mileageInput.Text;

            Car c = new Car(year, make, colour, mileage);
            inventory.Add(c);

            outputLabel.Text = yearInput.Text = makeInput.Text = colourInput.Text = mileageInput.Text = "";
            yearInput.Focus();
            displayItems();
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < inventory.Count; i++)
            {
                if (inventory[i].make == makeInput.Text)
                {
                    inventory.RemoveAt(i);
                }
                outputLabel.Text = yearInput.Text = makeInput.Text = colourInput.Text = mileageInput.Text = "";
            }

            int index = inventory.FindIndex(car => car.make == makeInput.Text);

            if (index >-1)
            {
                inventory.RemoveAt(index);
            }

            displayItems();
        }

        public void displayItems()
        {
            outputLabel.Text = "";

            foreach (Car c in inventory)
            {
                outputLabel.Text += c.year + " "
                     + c.make + " "
                     + c.colour + " "
                     + c.mileage + "\n";
            }
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            saveDB();
            Application.Exit();
        }

        public void loadDB()
        {
            string newYear, newMake, newColour, newMileage;

            XmlReader reader = XmlReader.Create("Resources/CarXML.xml", null);

           
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Text)
                {
                    newYear = reader.ReadString();

                    reader.ReadToNextSibling("make");
                    newMake = reader.ReadString();

                    reader.ReadToNextSibling("colour");
                    newColour = reader.ReadString();

                    reader.ReadToNextSibling("mileage");
                    newMileage = reader.ReadString();

                    Car c = new Car (newYear, newMake, newColour, newMileage);
                    inventory.Add(c);
                }
            }

            reader.Close();
        }

        public void saveDB()
        {
           
             XmlWriter writer = XmlWriter.Create("Resources/CarXML.xml", null);

            writer.WriteStartElement("Cars");

            foreach (Car c in inventory)
            {
                writer.WriteStartElement("Car");

                writer.WriteElementString("year", c.year);
                writer.WriteElementString("make", c.make);
                writer.WriteElementString("colour", c.colour);
                writer.WriteElementString("mileage", c.mileage);
                

                writer.WriteEndElement();
            }

            writer.WriteEndElement();
            writer.Close();
        }
    }
}
