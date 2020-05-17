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

export default function TableList() {
  const classes = useStyles();

  const [state, setState] = React.useState({
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
        title: 'Name', 
        field: 'name', 
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
        cellStyle: {
          width: 300,
          minWidth: 300
        },
        headerStyle: {
          width: 300,
          minWidth: 300
        }  
      },
      {
        title: 'Inventory',
        field: 'inventory',
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
    data: [
      { sku: '1010110000', 
        name: 'Thing 1', 
        description: 'Description Description Description 1', 
        inventory: 6 
      },
      { sku: '1010110000', 
        name: 'Thing 1', 
        description: 'Description Description Description 1', 
        inventory: 5 
      },
      { sku: '1010110000', 
        name: 'Thing 1', 
        description: 'Description Description Description 1', 
        inventory: 4
      },
      { sku: '1010110000', 
        name: 'Thing 1', 
        description: 'Description Description Description 1', 
        inventory: 2
      },
      { sku: '1010110000', 
        name: 'Thing 1', 
        description: 'Description Description Description 1', 
        inventory: 1
      },
      { sku: '1010110000', 
        name: 'Thing 1', 
        description: 'Description Description Description 1', 
        inventory: 10
      },
      { sku: '1010110000', 
        name: 'Thing 1', 
        description: 'Description Description Description 1', 
        inventory: 14
      },
      { sku: '1010110000', 
        name: 'Thing 1', 
        description: 'Description Description Description 1', 
        inventory: 12
      },
      {
        sku: '1010101001',
        name: 'Thing 2',
        description: 'Description Description Description 2',
        inventory: 50,
      },
    ],
  });

  return (
    <GridContainer>
      <GridItem xs={12} sm={12} md={12}>
        <Card>
          <CardHeader color="primary">
            <h4 className={classes.cardTitleWhite}>Supplies</h4>
            <p className={classes.cardCategoryWhite}>
              Here you can check your available supplies for your patients
            </p>
          </CardHeader>
          <CardBody>
            <MaterialTable
              title={false}
              columns={state.columns}
              data={state.data}
              editable={{
                onRowAdd: (newData) =>
                  new Promise((resolve) => {
                    setTimeout(() => {
                      resolve();
                      setState((prevState) => {
                        const data = [...prevState.data];
                        data.push(newData);
                        return { ...prevState, data };
                      });
                    }, 600);
                  }),
                onRowUpdate: (newData, oldData) =>
                  new Promise((resolve) => {
                    setTimeout(() => {
                      resolve();
                      if (oldData) {
                        setState((prevState) => {
                          const data = [...prevState.data];
                          data[data.indexOf(oldData)] = newData;
                          return { ...prevState, data };
                        });
                      }
                    }, 600);
                  }),
                onRowDelete: (oldData) =>
                  new Promise((resolve) => {
                    setTimeout(() => {
                      resolve();
                      setState((prevState) => {
                        const data = [...prevState.data];
                        data.splice(data.indexOf(oldData), 1);
                        return { ...prevState, data };
                      });
                    }, 600);
                  }),
              }}
            />
          </CardBody>
        </Card>
      </GridItem>
    </GridContainer>
  );
}
