import React from "react";
// @material-ui/core components
import { makeStyles } from "@material-ui/core/styles";
// core components
import GridItem from "components/Grid/GridItem.js";
import GridContainer from "components/Grid/GridContainer.js";
import MaterialTable from "material-table";
import Card from "components/Card/Card.js";
import CardHeader from "components/Card/CardHeader.js";
import CardBody from "components/Card/CardBody.js";
import axios from 'axios';
import moment from 'moment';

const styles = {
  cardCategoryWhite: {
    "&,& a,& a:hover,& a:focus": {
      color: "rgba(255,255,255,.62)",
      margin: "0",
      fontSize: "14px",
      marginTop: "0",
      marginBottom: "0"
    },
    "& a,& a:hover,& a:focus": {
      color: "#FFFFFF"
    }
  },
  cardTitleWhite: {
    color: "#FFFFFF",
    marginTop: "0px",
    minHeight: "auto",
    fontWeight: "300",
    fontFamily: "'Roboto', 'Helvetica', 'Arial', sans-serif",
    marginBottom: "3px",
    textDecoration: "none",
    "& small": {
      color: "#777",
      fontSize: "65%",
      fontWeight: "400",
      lineHeight: "1"
    }
  }
};

const useStyles = makeStyles(styles);
export default class Patients extends React.Component {
  constructor(props) {
      super(props);
      const classes = makeStyles(styles);
      this.state = {
        posts: []
      }
      this.state = {
        columns: [
          { 
        title: 'SKU', 
        field: 'sku', 
        cellStyle: {
          width: 20,
          minWidth: 20
        },
        headerStyle: {
          width: 20,
          minWidth: 20
        } 
      },
      { 
        title: 'Barcode', 
        field: 'barcode', 
        cellStyle: {
          width: 20,
          minWidth: 20
        },
        headerStyle: {
          width: 20,
          minWidth: 20
        } 
      },
      {
        title: 'Description',
        field: 'description',
        type: 'text',
        cellStyle: {
          width: 20,
          minWidth: 20
        },
        headerStyle: {
          width:20,
          maxWidth: 50
        } 
      },
      {
        title: 'Quantity',
        field: 'quantity',
        type: 'numeric',
        cellStyle: {
          width: 20,
          minWidth: 20
        },
        headerStyle: {
          width:20,
          maxWidth: 50
        } 
      },
    ],
        data: [],
        classes: classes
      };
    }
    fetchSupplies() {
      axios
      .get(`https://icuhelperreboot.azurewebsites.net/api/getInventory?code=lA6fYy1zs/vWtStuWlPoG/ZS/I8efNRU1FjHo5XnR/5qkhYX6KHB/A==`, {})
      .then(res => {
        const data = res.data.inventory;
        
        this.setState({
          data
        })
      })
    }
    componentWillMount(){
      this.fetchSupplies()
    }
    
    render() {
      return (
        <div>
          <GridContainer>
            <GridItem xs={12} sm={12} md={8}>
              <Card>
                <CardHeader color="primary">
                  <h4 className={this.state.classes.cardTitleWhite}>Patients Profile</h4>
                  <p className={this.state.classes.cardCategoryWhite}>Add or Modify Patients Data</p>
                </CardHeader>
                <CardBody>
                <MaterialTable
                  title={false}
                  columns={this.state.columns}
                  data={this.state.data}
                  editable={{
                    onRowAdd: (newData) =>
                      new Promise((resolve) => {
                        newData.sku = moment(newData.sku, "number").format("number");
                        newData.name = moment(newData.barcode, "text").format("text");
                        newData.description = moment(newData.description, "text").format("text");
                        newData.supplies_avaible = moment(newData.quantity, "number").format("number");
                        const url = "https://icuhelperfunctions.azurewebsites.net/api/addInventory?code=qGQr6Npysu/ZQVYRuxii0oz7y8DYU8mr7JbfstfKeiHFiht8yGi8uw=="
                        axios.post(url, newData)
                        .then( res => {
                          resolve();
                            const data = [...this.state.data];
                            data.push(newData);
                            this.state.data = { ...this.state.data, data };
                        }, 600);
                      }),
                    onRowUpdate: (newData, oldData) =>
                      new Promise((resolve, reject) => {
                        
                        setTimeout(() => {
                          const checkEmpty = Object.values(newData).filter( v => v.length === 0 );
                          if (checkEmpty.length !== 0) {
                            reject();
                            return;
                          } 
                          if (oldData) {
                            resolve();
                              const data = [...this.state.data];
                              data[data.indexOf(oldData)] = newData;
                              this.state.data = { ...this.state.data, data };
                          }
                        }, 600);
                      }),
                    onRowDelete: (oldData) =>
                      new Promise((resolve) => {
                        setTimeout(() => {
                          resolve();
                          const data = [...this.state.data];
                          data.splice(data.indexOf(oldData), 1);
                          this.state.data = { ...this.state.data, data };
                        }, 600);
                      }),
                  }}
                />
              </CardBody>
              </Card>
            </GridItem>
          </GridContainer>
        </div>
      );
    }
  }