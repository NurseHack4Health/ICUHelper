import React from "react";
// @material-ui/core components
import { makeStyles } from "@material-ui/core/styles";
// core components
import GridItem from "components/Grid/GridItem.js";
import GridContainer from "components/Grid/GridContainer.js";
import Button from "components/CustomButtons/Button.js";
import Card from "components/Card/Card.js";
import CardHeader from "components/Card/CardHeader.js";

import CardBody from "components/Card/CardBody.js";
import CardFooter from "components/Card/CardFooter.js";
import CardIcon from "components/Card/CardIcon.js";
import Accessibility from "@material-ui/icons/Accessibility";
import MaterialTable from "material-table"
import axios from 'axios';
import moment from 'moment';

const styles = {
  cardCategoryWhite: {
    color: "rgba(255,255,255,.62)",
    margin: "0",
    fontSize: "14px",
    marginTop: "0",
    marginBottom: "0"
  },
  cardTitleWhite: {
    color: "#FFFFFF",
    marginTop: "0px",
    minHeight: "auto",
    fontWeight: "300",
    fontFamily: "'Roboto', 'Helvetica', 'Arial', sans-serif",
    marginBottom: "3px",
    textDecoration: "none"
  },
  cardCategory: {
    color: "rgba(0,1,45,.65)",
    margin: "0",
    fontSize: "14px",
    marginTop: "0",
    marginBottom: "0"
  },
  cardTitle: {
    color: "rgba(0,1,45,.65)",
    margin: "0",
    fontSize: "14px",
    marginTop: "0",
    marginBottom: "0"
  },
};

const useStyles = makeStyles(styles);

export default class Patients extends React.Component {
  constructor(props) {
      super(props);
      const classes = makeStyles(styles);
      this.state = {
        columns: [
          { 
            title: 'Patient Name', 
            field: 'full_name', 
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
            title: 'Date Of Birth', 
            field: 'date_of_birth',
            type: 'date',
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
            title: 'Gender', 
            field: 'patient_gender', 
            cellStyle: {
              width: 10,
              minWidth: 10
            },
            headerStyle: {
              width: 10,
              minWidth: 10
            },
            lookup: {
              'M': 'Male',
              'F': 'Female'
            }
          },
          { 
            title: 'History Number', 
            field: 'history_number',
            type: 'numeric', 
            cellStyle: {
              width: 10,
              minWidth: 10
            },
            headerStyle: {
              width: 10,
              minWidth: 10
            } 
          },
          { 
            title: 'Treated', 
            field: 'patient_condition',
            cellStyle: {
              width: 100,
              minWidth: 100
            },
            headerStyle: {
              width: 100,
              minWidth: 100
            }  
          },
          { 
            title: 'Medication', 
            field: 'medication',
            cellStyle: {
              width: 100,
              minWidth: 100
            },
            headerStyle: {
              width: 100,
              minWidth: 100
            }  
          },
          {
            title: 'Condition',
            field: 'condition',
            cellStyle: {
              width: 20,
              minWidth: 20
            },
            headerStyle: {
              width:20,
              maxWidth: 20
            },
            lookup: { 
              1: "Good",
              2: "Fair",
              3: "Poor",
              4: "Critical",
              5: "Treated and Released",
              6: "Treated and Transferred",
              7: "Released",
              8: "Dead"
            },
          },
          {
            title: 'Using Ventilator',
            field: 'using_ventilator',
            type: 'boolean',
            cellStyle: {
              width: 20,
              minWidth: 20
            },
            headerStyle: {
              width:20,
              maxWidth: 10
            } 
          },
        ],
        data: [],
        classes: classes
      };
    }
    fetchPatients() {
      axios
      .get(`https://icuhelperfunctions.azurewebsites.net/api/getPatientsAssigned?code=vvanYgV6CrStwWIn0nyjAT73dOkGDa/duSDTm8bDOXPcOaYBpJ0aaw==`, {})
      .then(res => {
        const data = res.data.inventory;
        this.setState({
          data
        })
      })
    }
    componentWillMount(){
      this.fetchPatients()
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
                          newData.date_of_birth = moment(newData.date_of_birth, "DD.MM.YYYY").format("YYYY-MM-DD");
                          newData.password = "hola";
                          newData.email = "hola@gmail.com";
                          const url = "https://icuhelperfunctions.azurewebsites.net/api/AddPatient?code=8lmbKjnucT1MQaEeRJsCfB5QNXLsjmbTmr4lXgECmhjRp5buSQdnLw=="
                          axios.post(url, newData)
                          .then( res => {
                            resolve();
                            const data = [...this.state.data];
                            data.push(newData);
                            this.state.data = { ...this.state.data, data };
                          })
                          .catch( error => {
                            console.log("Error: ", error);
                          })
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
    };

}
