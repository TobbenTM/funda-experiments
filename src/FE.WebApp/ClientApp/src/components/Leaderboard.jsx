import React, { Component } from 'react';
import * as signalR from '@aspnet/signalr';

export default class Leaderboard extends Component {

  constructor(props) {
    super(props);
    this.state = {
      totalAdsToCalculate: null,
      totalAdsCalculated: null,
      doneCalculating: false,
      leaderboard: [],
      error: null,
      hubConnection: null,
    };
  }

  componentDidMount = () => {
    const hubConnection = new signalR.HubConnectionBuilder()
      .withUrl("/ws/topten")
      .build();

    this.setState({ hubConnection }, () => {
      // Register a listener for new leaderboard updates
      this.state.hubConnection.on('LeaderboardUpdated', topTen => this.setState({
        totalAdsToCalculate: topTen.totalAdsToCalculate,
        totalAdsCalculated: topTen.totalAdsCalculated,
        doneCalculating: topTen.doneCalculating,
        leaderboard: topTen.leaderboard,
      }));

      // Connect to the hub, then invoke the calculation
      this.state.hubConnection.start()
        .then(() => {
          this.state.hubConnection.invoke('StartCalculating')
            .catch(error => this.setState({ error }));
        });
    });
  }

  render() {
    if (this.state.error) {
      return (<span><h2>Error!</h2><br /><p>Could not complete calculation: {this.state.error.message}</p></span>);
    }

    return (
      <div>
        <ol>
          {this.state.leaderboard.map(agent => (
            <li>{agent.name}, {agent.numberOfAds} ads</li>
          ))}
        </ol>
        <p>{this.state.totalAdsCalculated} of {this.state.totalAdsToCalculate} calculated</p>
      </div>
    );
  }
}
