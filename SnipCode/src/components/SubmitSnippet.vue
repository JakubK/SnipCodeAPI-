<template>
  <div @click.self="handleClose()" class="submit-background">
    <div class="submit-panel">
      <h1>Before we continue...</h1>
      <p>Please name your Snippet so it will be easier to identify it. </p>
      <form>
        <label>Snippet's name</label>
        <input v-model="name" name="name" type="text" placeholder="Unnamed Snippet"/>
        <br/>
        <button @click="handleSubmit" type="button">Submit your Snippet</button>
      </form>
    </div>
  </div>
</template>
<script>
export default {
  props: ['content'],
  name: 'SubmitSnippet',
  data()
  {
    return{
      name: ''
    }
  },
  methods:
  {
    handleSubmit()
    {
       this.$store.dispatch("uploadSnippetContent",{content: this.content,name: this.name}).then(result =>
       {
         this.$router.push(result.data.hash);
       });
    },
    handleClose()
    {
      this.$router.go(-1);
    }
  }
}
</script>
<style lang="scss" src='./SubmitSnippet.scss' scoped></style>