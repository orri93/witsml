﻿using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PDS.Witsml.Studio.Connections;

namespace PDS.Witsml.Studio
{
    /// <summary>
    /// Unit tests for the ConnectionViewModel
    /// </summary>
    [TestClass]
    public class ConnectionViewModelTests : ConnectionViewModelTestBase
    {
        /// <summary>
        /// Tests the connection filename for each type of connection has the correct prefix.
        /// </summary>
        [TestMethod]
        public void TestConnectionFilenamePrefix()
        {
            // Test Witsml filename
            var filename = Path.GetFileName(_witsmlConnectionVm.GetConnectionFilename());
            Assert.IsTrue(filename.StartsWith(ConnectionTypes.Witsml.ToString()));

            // Test Etp filename
            filename = Path.GetFileName(_etpConnectionVm.GetConnectionFilename());
            Assert.IsTrue(filename.StartsWith(ConnectionTypes.Etp.ToString()));
        }

        /// <summary>
        /// Tests the connection filename is correctly generated with the 
        /// ConnectionType prefix and the base file name.
        /// </summary>
        [TestMethod]
        public void TestConnectionFilename()
        {
            var witsmlFilename = string.Format("{0}{1}", ConnectionTypes.Witsml, ConnectionBaseFileName);
            var etpFilename = string.Format("{0}{1}", ConnectionTypes.Etp, ConnectionBaseFileName);

            // Test Witsml filename
            var filename = Path.GetFileName(_witsmlConnectionVm.GetConnectionFilename());
            Assert.AreEqual(witsmlFilename, filename);

            // Test Etp filename
            filename = Path.GetFileName(_etpConnectionVm.GetConnectionFilename());
            Assert.AreEqual(etpFilename, filename);
        }

        /// <summary>
        /// Tests that a null connection is returned when no connection file is persisted.
        /// </summary>
        [TestMethod]
        public void TestNoConnectionFilePersisted()
        {
            Assert.IsNull(_witsmlConnectionVm.OpenConnectionFile());
            Assert.IsNull(_etpConnectionVm.OpenConnectionFile());
        }

        /// <summary>
        /// Tests the equivalence of a connection before and after file persistence.
        /// </summary>
        [TestMethod]
        public void TestConnectionFilePersistence()
        {
            _witsmlConnectionVm.SaveConnectionFile(_witsmlConnection);
            _etpConnectionVm.SaveConnectionFile(_etpConnection);

            var witsmlRead = _witsmlConnectionVm.OpenConnectionFile();
            Assert.AreEqual(_witsmlConnection.Uri, witsmlRead.Uri);

            var etpRead = _etpConnectionVm.OpenConnectionFile();
            Assert.AreEqual(_etpConnection.Uri, etpRead.Uri);
        }

        [TestMethod]
        public void TestInitialeEditItemNoDataItem()
        {
            var emptyConnection = new Connection();
            _witsmlConnectionVm.InitializeEditItem();

            Assert.AreEqual(emptyConnection.Uri, _witsmlConnectionVm.EditItem.Uri);
        }

        [TestMethod]
        public void TestInitialeEditItemWithDataItem()
        {
            _witsmlConnectionVm.DataItem = _witsmlConnection;
            _witsmlConnectionVm.InitializeEditItem();

            Assert.AreEqual(_witsmlConnection.Uri, _witsmlConnectionVm.EditItem.Uri);
        }

        [TestMethod]
        public void TestInitialeEditItemWithPersistedConnection()
        {
            _witsmlConnectionVm.SaveConnectionFile(_witsmlConnection);
            _witsmlConnectionVm.InitializeEditItem();

            Assert.AreEqual(_witsmlConnection.Uri, _witsmlConnectionVm.EditItem.Uri);
        }

        [TestMethod]
        public void TestCancelWithDataItem()
        {
            var newName = "xxx";

            // Initialze the Edit Item by setting the DataItem
            _witsmlConnectionVm.DataItem = _witsmlConnection;
            _witsmlConnectionVm.InitializeEditItem();

            // Make a change to the edit item
            _witsmlConnectionVm.EditItem.Name = newName;

            // Accept the changes
            _witsmlConnectionVm.Cancel();

            // Test that the newName was not assigned
            Assert.AreNotEqual(newName, _witsmlConnectionVm.DataItem.Name);

            // Test that the Name is unchanged
            Assert.AreEqual(_witsmlConnection.Name, _witsmlConnectionVm.DataItem.Name);
        }
    }
}
