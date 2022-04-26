<template>
  <div class="time-tracker">
    <v-tooltip bottom content-class="custom-tooltip">
            <template v-slot:activator="{ on, attrs }">
              <div v-bind="attrs" v-on="on">
                <v-icon dense color="grey lighten-1">mdi-alarm</v-icon>
                <span class="unselectable font-weight-medium ml-2" :class="timerColor+'--text'">{{ timeLeft }}</span>
              </div>
            </template>
            <span>Test končí {{ endMoment }}</span>
          </v-tooltip>
  </div>
</template>

<script>
import moment from 'moment';

moment.locale("cs");

export default {
    name: "TimeRemainingTracker",
    props: {
      end: {
        type: String,
        required: true
      }
    },
    data() {
      return {
        endMoment: moment.utc(this.end).local().format("LLL")
      }
    },
    computed: {
      currentTimeUtc() {
        return this.$store.state.currentTimeUtc;
      },
      duration() {
        if(moment.utc(this.end) > this.currentTimeUtc) {
          return moment.duration(moment.utc(this.end).diff(this.currentTimeUtc));
        }
        else return moment.duration(0);
      },
      timerColor() {
        if(this.duration.asMinutes() < 1) {
          return 'red'
        }
        if(this.duration.asMinutes() < 10) {
          return 'orange'
        }
        return ""
      },
      timeLeft() {
        let duration = this.duration;
        let days = duration.days();
        if(days > 0) {
          return "Zbýva " + duration.locale("cs-cz").humanize();
        }
        let hours = duration.hours();
        if( hours < 10) {
          hours = '0' + hours;
        }
        let minutes = duration.minutes();
        if( minutes < 10) {
          minutes = '0' + minutes;
        }
        let seconds = duration.seconds();
        if( seconds < 10) {
          seconds = '0' + seconds;
        }
        let formattedTime =  hours + ":" + minutes + ":" + seconds
        return formattedTime;
      },
      
    },
    watch: {
      duration(newValue, oldValue) {
        if (oldValue > 0 && newValue == 0) {
          this.$emit('duration-over');
        }
      }
    }
}
</script>

<style>
.time-tracker {
  justify-content: center;
  align-content: center;
}
.unselectable {
  -webkit-user-select: none; /* Safari */
  -moz-user-select: none; /* Firefox */
  -ms-user-select: none; /* IE10+/Edge */
  user-select: none; /* Standard */
}
</style>