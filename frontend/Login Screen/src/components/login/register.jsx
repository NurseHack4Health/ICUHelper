import React, { Component } from 'react';
import LoginImg from "../../login.svg";
import axios from "axios";

export  class Register extends Component {

    constructor(props) {
        super(props);

        this.state = {
            email: "",
            password: "",
            password_confirmation: "",
            registrationErrors: ""
          };
      
          this.handleSubmit = this.handleSubmit.bind(this);
          this.handleChange = this.handleChange.bind(this);
    }

    handleChange(event) {
        this.setState({
          [event.target.name]: event.target.value
        });
      }
    
      handleSubmit(event) {
        const { email, password, password_confirmation } = this.state;
    
        axios
          .post(
            "http://localhost:3001/registrations",
            {
              user: {
                email: email,
                password: password,
                password_confirmation: password_confirmation
              }
            },
            { withCredentials: true }
          )
          .then(response => {
            if (response.data.status === "created") {
              this.props.handleSuccessfulAuth(response.data);
            }
          })
          .catch(error => {
            console.log("registration error", error);
          });
        event.preventDefault();
      }

    render() {
        return (
            <div className="base-container" ref={this.props.containerRef}>
                <form onSubmit={this.handleSubmit}>
                <div className="header">Register</div>
                <div className="content">
                    <div className="image">
                        <img src={LoginImg} />
                    </div>
                    <div className="form">
                        <div className="form-group">
                            <label htmlFor="email">Email</label>
                            <input type="text" name="email"
                             placeholder="Email"
                             values={this.state.email}
                             onChange={this.handleChange}
                             required />
                        </div>
                        <div className="form-group">
                            <label htmlFor="password">Password</label>
                            <input type="text" name="password"
                             placeholder="Password"
                             value={this.state.password}
                             onChange={this.handleChange}
                              required />
                        </div>
                        <div className="form-group">
                            <label htmlFor="password_confirmation">Password Confirmation</label>
                            <input type="password" name="password_confirmation"
                             placeholder="Password confirmation"
                             value={this.state.password_confirmation}
                              onChange={this.handleChange}
                               required />
                        </div>
                    </div>
                </div>
                <div className="footer">
                    <button type="button" className="btn">
                        Register
                        </button>
                </div>
                </form>
            </div>
        );
    }
}
